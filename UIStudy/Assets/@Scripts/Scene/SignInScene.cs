using UnityEngine;
using UnityEngine.EventSystems;
using static Define;
using GameApi.Dtos;
using System.Collections;
using System;
using WebApi.Models.Dto;

public class SignInScene  : BaseScene
{
    // private bool _isLoadSceneCondition = false;
    // private bool _isLoadEnergyCondition = false;
    
    // private int _failCount = 0;

    //private EScene _loadScene = EScene.SuberunkerSceneHomeScene;
    private UI_SignInScene _ui;
    // private string _id;
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        _ui = Managers.UI.ShowSceneUI<UI_SignInScene>();
        Systems.GoogleLoginWebView.OnGetGoogleAccount -= GoogleAccountSignin; // 구독 해제
        Systems.GoogleLoginWebView.OnGetGoogleAccount += GoogleAccountSignin; // 이벤트 구독

        return true;
    }

    void OnDestroy()
    {
        Systems.GoogleLoginWebView.OnGetGoogleAccount -= GoogleAccountSignin; // 구독 해제
    }

    public void GoogleAccountSignin(string googleAccount)
    {
        Managers.Login.ProcessGoogleAccount(googleAccount);
        // Debug.Log("Event_GoogleAccountSignin");
        // Managers.WebContents.CheckGoogleAccountExists(new ReqDtoGoogleAccount()
        // {
        //     GoogleAccount = googleAccount
        // }, (response) =>
        // {
        //     // 새로운 계정 (중복 없음)
        //     Managers.Game.UserInfo.GoogleAccount = googleAccount;
        //     Managers.Scene.LoadScene(EScene.InputNicknameScene);
        // }, (errorCode) =>
        // {
        //     //Debug.Log($"errorCode: {errorCode} (int 값: {(int)errorCode})");
        //     //Debug.Log($"GoogleAccountAlreadyExists 값: {(int)EStatusCode.GoogleAccountAlreadyExists}");
        
        //     if (errorCode == EStatusCode.GoogleAccountAlreadyExists)
        //     {
        //         // 이미 등록된 계정이면
        //         Debug.Log($"googleAccount : {googleAccount}");
        //         Managers.Game.UserInfo.GoogleAccount = googleAccount;
        //         Managers.Login.SignInWithGoogle();
        //         // 바로 로그인 시켜주기
        //     }
        //     else
        //     {
        //         // 다른 오류일 경우 (ServerException 등)
        //         Debug.LogError($"로그인 중 오류 발생: {errorCode}");
        //         // 여기에 오류 팝업 표시 코드 추가
        //     }
        // });
    }

    // public void SignIn()
    // {
    //     var loadingComplete = UI_LoadingPopup.Show();
    //     Managers.WebContents.GetUserAccount(new ReqDtoGetUserAccount()
    //     {
    //         UserName = Managers.Game.UserInfo.UserName,
    //         Password = Managers.Game.UserInfo.Password
    //     },
    //     (response) =>
    //     {
    //         Managers.Game.UserInfo.UserName = response.UserName;
    //         Managers.Game.UserInfo.UserNickname = response.Nickname;
    //         Managers.Game.UserInfo.UserAccountId = response.UserAccountId;
    //         Managers.Game.UserInfo.GoogleAccount = response.GoogleAccount;
    //         Managers.SignalR.LoginUser(Managers.Game.UserInfo.UserAccountId);

    //         //캐릭터 스타일 저장
    //         Managers.Game.ChracterStyleInfo.CharacterId = response.CharacterId;
    //         Managers.Game.ChracterStyleInfo.Hair = response.HairStyle;
    //         Managers.Game.ChracterStyleInfo.Eyebrows = response.EyebrowStyle;
    //         Managers.Game.ChracterStyleInfo.Eyes = response.EyesStyle;
    //         Managers.Game.UserInfo.EvolutionId = response.Evolution;
    //         Managers.Game.UserInfo.EvolutionSetLevel = response.EvolutionSetLevel;

    //         // Energy
    //         Managers.Game.UserInfo.Energy = response.Energy;
    //         Managers.Game.UserInfo.LatelyEnergy = response.LatelyEnergy;
    //         Managers.Game.UserInfo.PurchaseEnergyCountToday = response.PurchaseEnergyCountToday;
            
    //         // 일일 보상
    //         Managers.Game.UserInfo.LastRewardClaimTime = response.LastRewardClaimTime;
            
    //         //게임 진행 정보
    //         Managers.Game.UserInfo.RecordScore = response.HighScore;
    //         Managers.Game.UserInfo.LatelyScore = response.LatelyScore;
    //         Managers.Game.UserInfo.Gold = response.Gold;
    //         Managers.Game.UserInfo.PlayTime = response.PlayTime;
    //         Managers.Game.UserInfo.AccumulatedStone = response.AccumulatedStone;
    //         Managers.Game.UserInfo.StageLevel = response.StageLevel;
            
    //         // 보안 키 저장
    //         //SecurePlayerPrefs.SetKey(response.SecureKey);

    //         Managers.Event.TriggerEvent(EEventType.OnSettlementComplete);
    //         Managers.Event.TriggerEvent(EEventType.OnFirstAccept);

    //         // 아이디 저장
    //         Managers.Login.SaveUserAccountInfo();

    //         _isLoadEnergyCondition = true;
    //     },
    //     (errorCode) =>
    //     {
    //         UI_ToastPopup.ShowError(Managers.Error.GetError(EErrorCode.ERR_InvalidCredentials));
    //     });

    //     //1. 다른버튼 비활성화
    //     //2. 로딩 인디케이터
    //     {
    //         //StartCoroutine(LoadScore_Co());
    //         StartCoroutine(UpdateEnergy());

    //         loadingComplete.Value = true;
    //     }
    // }
    // public void SignInWithGoogle()
    // {
    //     Debug.Log($"SignInWithGoogle: {Managers.Game.UserInfo.GoogleAccount}");

    //     var loadingComplete = UI_LoadingPopup.Show();
    //     Managers.WebContents.GetUserAccountByGoogle(new ReqDtoGetUserAccountByGoogle()
    //     {
    //         GoogleAccount = Managers.Game.UserInfo.GoogleAccount
    //     },
    //     (response) =>
    //     {
    //         Managers.Game.UserInfo.UserName = response.UserName;
    //         Managers.Game.UserInfo.UserNickname = response.Nickname;
    //         Managers.Game.UserInfo.UserAccountId = response.UserAccountId;
    //         Managers.Game.UserInfo.GoogleAccount = response.GoogleAccount;
    //         Managers.SignalR.LoginUser(Managers.Game.UserInfo.UserAccountId);

    //         //캐릭터 스타일 저장
    //         Managers.Game.ChracterStyleInfo.CharacterId = response.CharacterId;
    //         Managers.Game.ChracterStyleInfo.Hair = response.HairStyle;
    //         Managers.Game.ChracterStyleInfo.Eyebrows = response.EyebrowStyle;
    //         Managers.Game.ChracterStyleInfo.Eyes = response.EyesStyle;
    //         Managers.Game.UserInfo.EvolutionId = response.Evolution;
    //         Managers.Game.UserInfo.EvolutionSetLevel = response.EvolutionSetLevel;

    //         // Energy
    //         Managers.Game.UserInfo.Energy = response.Energy;
    //         Managers.Game.UserInfo.LatelyEnergy = response.LatelyEnergy;
    //         Managers.Game.UserInfo.PurchaseEnergyCountToday = response.PurchaseEnergyCountToday;

    //         // 일일 보상
    //         Managers.Game.UserInfo.LastRewardClaimTime = response.LastRewardClaimTime;

    //         //게임 진행 정보
    //         Managers.Game.UserInfo.RecordScore = response.HighScore;
    //         Managers.Game.UserInfo.LatelyScore = response.LatelyScore;
    //         Managers.Game.UserInfo.Gold = response.Gold;
    //         Managers.Game.UserInfo.PlayTime = response.PlayTime;
    //         Managers.Game.UserInfo.AccumulatedStone = response.AccumulatedStone;
    //         Managers.Game.UserInfo.StageLevel = response.StageLevel;

    //         Managers.Login.SaveUserAccountInfo();

    //         Managers.Event.TriggerEvent(EEventType.OnSettlementComplete);
    //         Managers.Event.TriggerEvent(EEventType.OnFirstAccept);

    //         _isLoadEnergyCondition = true;
    //     },
    //     (errorCode) =>
    //     {
    //         Debug.LogError($"구글 계정 로그인 실패: {errorCode}");
    //         UI_ToastPopup.ShowError(Managers.Error.GetError(EErrorCode.ERR_InvalidCredentials));
    //     });

    //     {
    //         StartCoroutine(UpdateEnergy());
    //         loadingComplete.Value = true;
    //     }
    // }
    public void LoadSignUp()
    {
        //_loadScene = EScene.SignUpScene;
        Managers.Scene.LoadScene(EScene.SignUpScene);
    }

    // private IEnumerator LoadScene_Co()
    // {
    //     yield return new WaitWhile(() => _isLoadSceneCondition == false);
    //     Managers.Scene.LoadSceneWithProgress(_loadScene, "PreLoad");
    // }

    // private IEnumerator UpdateEnergy()
    // {
    //     yield return new WaitWhile(() => _isLoadEnergyCondition == false);

    //     var loadingComplete = UI_LoadingPopup.Show();

    //     Managers.WebContents.UpdateEnergy(new ReqDtoUpdateEnergy()
    //     {
    //         UserAccountId = Managers.Game.UserInfo.UserAccountId
    //     },
    //     (response) =>
    //     {
    //         loadingComplete.Value = true;
    //         Debug.Log("log in" + Managers.Game.UserInfo.LatelyEnergy);
    //         Managers.Game.UserInfo.Energy = response.Energy;
    //         Managers.Game.UserInfo.LatelyEnergy = response.LatelyEnergy;
    //         _isLoadSceneCondition = true;
    //         StartCoroutine(LoadScene_Co());
    //     },
    //     (errorCode) =>
    //     {
    //         UI_ToastPopup.ShowError(Managers.Error.GetError(EErrorCode.ERR_NetworkSaveError));
    //         HandleFailure();
    //     }
    //     );
    // }

    // private void HandleFailure()
    // {
    //     if (_failCount < HardCoding.MAX_FAIL_COUNT)
    //     {
    //         _failCount++;
    //         StartCoroutine(UpdateEnergy());
    //         return;
    //     }
    //     _failCount = 0;
    //     _loadScene = EScene.SignInScene;
    //     Managers.Scene.LoadScene(_loadScene);
    // }

}
