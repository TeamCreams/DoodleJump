using UnityEngine;
using UnityEngine.EventSystems;
using static Define;
using GameApi.Dtos;
using System.Collections;
using System;

public class SignInScene  : BaseScene
{
    private bool _isLoadSceneCondition = false;
    private bool _isLoadEnergyCondition = false;
    
    private int _failCount = 0;

    private EScene _loadScene = EScene.SuberunkerSceneHomeScene;
    private UI_SignInScene _ui;
    // private string _id;
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        _ui = Managers.UI.ShowSceneUI<UI_SignInScene>();
        return true;
    }
    public void SignIn()
    {
        
        // EErrorCode error = _ui.CheckCorrectPassword();
        // if (error != EErrorCode.ERR_OK)
        // {
        //     return;
        // }
        
        var loadingComplete = UI_LoadingPopup.Show();
        Managers.WebContents.GetUserAccount(new ReqDtoGetUserAccount()
        {
            UserName = Managers.Game.UserInfo.UserName,
            Password = Managers.Game.UserInfo.Password
        },
        (response) =>
        {
            Managers.Game.UserInfo.UserName = response.UserName;
            Managers.Game.UserInfo.UserNickname = response.Nickname;
            Managers.Game.UserInfo.UserAccountId = response.UserAccountId;
            Managers.SignalR.LoginUser(Managers.Game.UserInfo.UserAccountId);

            //캐릭터 스타일 저장
            Managers.Game.ChracterStyleInfo.CharacterId = response.CharacterId;
            Managers.Game.ChracterStyleInfo.Hair = response.HairStyle;
            Managers.Game.ChracterStyleInfo.Eyebrows = response.EyebrowStyle;
            Managers.Game.ChracterStyleInfo.Eyes = response.EyesStyle;
            Managers.Game.UserInfo.EvolutionId = response.Evolution;
            Managers.Game.UserInfo.EvolutionSetLevel = response.EvolutionSetLevel;

            // Energy
            Managers.Game.UserInfo.Energy = response.Energy;
            Managers.Game.UserInfo.LatelyEnergy = response.LatelyEnergy;
            Managers.Game.UserInfo.PurchaseEnergyCountToday = response.PurchaseEnergyCountToday;
            
            // 일일 보상
            Managers.Game.UserInfo.LastRewardClaimTime = response.LastRewardClaimTime;
            
            //게임 진행 정보
            Managers.Game.UserInfo.RecordScore = response.HighScore;
            Managers.Game.UserInfo.LatelyScore = response.LatelyScore;
            Managers.Game.UserInfo.Gold = response.Gold;
            Managers.Game.UserInfo.PlayTime = response.PlayTime;
            Managers.Game.UserInfo.AccumulatedStone = response.AccumulatedStone;
            Managers.Game.UserInfo.StageLevel = response.StageLevel;
            
            // 보안 키 저장
            //SecurePlayerPrefs.SetKey(response.SecureKey);

            Managers.Event.TriggerEvent(EEventType.OnSettlementComplete);
            Managers.Event.TriggerEvent(EEventType.OnFirstAccept);

            // 아이디 저장
            SecurePlayerPrefs.SetString(HardCoding.UserName, Managers.Game.UserInfo.UserName);
            SecurePlayerPrefs.SetString(HardCoding.Password, Managers.Game.UserInfo.Password);
            SecurePlayerPrefs.Save();

            _isLoadEnergyCondition = true;
        },
        (errorCode) =>
        {
            UI_ToastPopup.ShowError(Managers.Error.GetError(EErrorCode.ERR_InvalidCredentials));
        });

        //1. 다른버튼 비활성화
        //2. 로딩 인디케이터
        {
            //StartCoroutine(LoadScore_Co());
            StartCoroutine(UpdateEnergy());

            loadingComplete.Value = true;
        }
    }

    public void LoadSignUp()
    {
        _loadScene = EScene.SignUpScene;
        Managers.Scene.LoadScene(_loadScene);
    }

    private IEnumerator LoadScene_Co()
    {
        yield return new WaitWhile(() => _isLoadSceneCondition == false);
        Managers.Scene.LoadSceneWithProgress(_loadScene, "PreLoad");
    }

    // private IEnumerator LoadScore_Co()
    // {
    //     yield return new WaitWhile(() => _isLoadScoreCondition == false);
    //     Managers.Score.GetScore((this), null,
    //     () =>
    //     {
    //         _loadScene = EScene.SuberunkerSceneHomeScene;
    //         _isLoadEnergyCondition = true;
    //     },
    //     () =>
    //     {
    //         _loadScene = EScene.SignInScene;
    //     });
    // }

    // public IEnumerator CheckLogId_Co(string id, Action<string> callback)
    // {
    //     // 비밀번호 값이 생길 때까지 기다린 후 반환
    //     var loadingPopup = Managers.UI.ShowPopupUI<UI_LoadingPopup>();
    //     string password = "";
    //     bool isCompleted = false;
    //     Managers.WebContents.ReqGetUserAccount(new ReqDtoGetUserAccount()
    //     {
    //         UserName = id,
    //     },
    //     (response) =>
    //     {
    //         _id = id;
    //         password = response.Password;
    //         isCompleted = true;
    //     },
    //     (errorCode) =>
    //     {
    //         password = "X";
    //         isCompleted = true;
    //     });
    //     Managers.UI.ClosePopupUI(loadingPopup);
    //     yield return new WaitWhile(() => isCompleted == false);

    //     callback(password);
    // }

    // private void GetUserInfo()
    // {
    //     // 1. 비밀번호 인풋필드를 누른다.
    //     // 2. 아이디를 조회한다.
    //     //      - 있음 -> 로그인 가능
    //     //      - 없음 -> 불가.
    //     var loadingPopup = Managers.UI.ShowPopupUI<UI_LoadingPopup>();

    //     Managers.WebContents.ReqGetUserAccount(new ReqDtoGetUserAccount()
    //     {
    //         UserName = _id,
    //     },
    //    (response) =>
    //    {
    //        // 유저 정보 저장
    //        Managers.Game.UserInfo.UserName = response.UserName;
    //        Managers.Game.UserInfo.UserNickname = response.Nickname;
    //        Managers.Game.UserInfo.UserAccountId = response.UserAccountId;
    //         Managers.SignalR.LoginUser(Managers.Game.UserInfo.UserAccountId);

    //        //캐릭터 스타일 저장
    //        Managers.Game.ChracterStyleInfo.CharacterId = response.CharacterId;
    //        Managers.Game.ChracterStyleInfo.Hair = response.HairStyle;
    //        Managers.Game.ChracterStyleInfo.Eyebrows = response.EyebrowStyle;
    //        Managers.Game.ChracterStyleInfo.Eyes = response.EyesStyle;
    //        Managers.Game.UserInfo.EvolutionId = response.Evolution;
    //        Managers.Game.UserInfo.Energy = response.Energy;
    //        Managers.Game.UserInfo.LatelyEnergy = response.LatelyEnergy;

    //        Managers.Event.TriggerEvent(EEventType.OnSettlementComplete);
    //        Managers.Event.TriggerEvent(EEventType.OnFirstAccept);

    //        // 아이디 저장
    //        PlayerPrefs.SetString(HardCoding.UserName, Managers.Game.UserInfo.UserName);
    //        PlayerPrefs.Save();

    //        _isLoadScoreCondition = true;
    //    },
    //    (errorCode) =>
    //    {
    //         UI_ToastPopup toast = Managers.UI.ShowPopupUI<UI_ToastPopup>();
    //         ErrorStruct errorStruct = Managers.Error.GetError(EErrorCode.ERR_NetworkIDError);
    //         toast.SetInfo(errorStruct.Notice, UI_ToastPopup.Type.Error);
    //    });
    //     Managers.UI.ClosePopupUI(loadingPopup);
    // }

    private IEnumerator UpdateEnergy()
    {
        yield return new WaitWhile(() => _isLoadEnergyCondition == false);

        var loadingComplete = UI_LoadingPopup.Show();

        Managers.WebContents.UpdateEnergy(new ReqDtoUpdateEnergy()
        {
            UserAccountId = Managers.Game.UserInfo.UserAccountId
        },
        (response) =>
        {
            loadingComplete.Value = true;
            Debug.Log("log in" + Managers.Game.UserInfo.LatelyEnergy);
            Managers.Game.UserInfo.Energy = response.Energy;
            Managers.Game.UserInfo.LatelyEnergy = response.LatelyEnergy;
            _isLoadSceneCondition = true;
            StartCoroutine(LoadScene_Co());
        },
        (errorCode) =>
        {
            UI_ToastPopup.ShowError(Managers.Error.GetError(EErrorCode.ERR_NetworkSaveError));
            HandleFailure();
        }
        );
    }

    private void HandleFailure()
    {
        if (_failCount < HardCoding.MAX_FAIL_COUNT)
        {
            _failCount++;
            StartCoroutine(UpdateEnergy());
            return;
        }
        _failCount = 0;
        _loadScene = EScene.SignInScene;
        Managers.Scene.LoadScene(_loadScene);
    }
}
