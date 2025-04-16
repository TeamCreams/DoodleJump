using System.Collections;
using GameApi.Dtos;
using UnityEngine;
using static Define;

public class SignUpScene : BaseScene
{
    private bool _isLoadSceneCondition = false;
    private bool _isLoadEnergyCondition = false;
    
    private int _failCount = 0;

    private EScene _loadScene = EScene.SuberunkerSceneHomeScene;
    
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        Managers.UI.ShowSceneUI<UI_SignUpScene>();
        Managers.Event.AddEvent(EEventType.GoogleSignup,Event_GoogleAccountSignup);

        return true;
    }

    void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.GoogleSignup,Event_GoogleAccountSignup);
    }

    void Event_GoogleAccountSignup(Component sender, object param)
    {
        Managers.WebContents.CheckGoogleAccountExists(new ReqDtoGoogleAccount()
        {
            GoogleAccount = Managers.Game.UserInfo.GoogleAccount
        },(response) =>
        {
            Managers.Scene.LoadScene(EScene.InputNicknameScene);       
        },(errorCode) =>
        {       
            // 바로 로그인 시켜주기
        });
    }
    private IEnumerator LoadScene_Co()
    {
        yield return new WaitWhile(() => _isLoadSceneCondition == false);
        Managers.Scene.LoadSceneWithProgress(_loadScene, "PreLoad");
    }
    public void SignIn()
    {
        var loadingComplete = UI_LoadingPopup.Show();
        Managers.WebContents.GetUserAccount(new ReqDtoGetUserAccount()
        {
            UserName = Managers.Game.UserInfo.UserName,
            Password = Managers.Game.UserInfo.Password,
            GoogleAccount = Managers.Game.UserInfo.GoogleAccount
        },
        (response) =>
        {
            Managers.Game.UserInfo.UserName = response.UserName;
            Managers.Game.UserInfo.UserNickname = response.Nickname;
            Managers.Game.UserInfo.UserAccountId = response.UserAccountId;
            Managers.Game.UserInfo.GoogleAccount = response.GoogleAccount;
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
            SecurePlayerPrefs.SetString(HardCoding.GoogleAccount, Managers.Game.UserInfo.GoogleAccount);
            
            SecurePlayerPrefs.Save();

            _isLoadEnergyCondition = true;
        },
        (errorCode) =>
        {
            UI_ToastPopup.ShowError(Managers.Error.GetError(EErrorCode.ERR_InvalidCredentials));
        });

        {
            StartCoroutine(UpdateEnergy());

            loadingComplete.Value = true;
        }
    }

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
