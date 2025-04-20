using System;
using System.Collections;
using GameApi.Dtos;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using WebApi.Models.Dto;
using static Define;

public class StartLoadingScene : BaseScene
{
    private int _failCount = 0;
    private EScene _scene = EScene.SignInScene;
    private bool _isPreLoadSuccess = false;
    private bool _isLoadSceneCondition = false;

    private PlayableDirector _playableDirector = null;
    private UI_StartLoadingScene _ui = null;
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        //_ui = Managers.UI.ShowSceneUI<UI_StartLoadingScene>();
        GameObject ui = GameObject.Find("UI_StartLoadingScene");
        _ui = ui.GetOrAddComponent<UI_StartLoadingScene>();

        _playableDirector = this.gameObject.GetOrAddComponent<PlayableDirector>();
        _playableDirector.playableAsset = _ui.GetOrAddComponent<PlayableDirector>().playableAsset;
         
        _playableDirector.stopped += OnPlayableDirectorStopped;
        StartLoadAssets("PreLoad");
        return true;
    }

    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        StartCoroutine(LoadUserAccount_Co());
    }

    private IEnumerator LoadUserAccount_Co()
    {
        yield return new WaitWhile(() => _isPreLoadSuccess == false);
        OnEvent_LoadUserAccount(); 
    }

    public void OnEvent_LoadUserAccount()
    {
        string usernameData = SecurePlayerPrefs.GetString(HardCoding.UserName, "");
        string passwordData = SecurePlayerPrefs.GetString(HardCoding.Password, ""); 
        string googleAccountData = SecurePlayerPrefs.GetString(HardCoding.GoogleAccount, ""); 
        Managers.Game.UserInfo.UserName = usernameData;
        Managers.Game.UserInfo.Password = passwordData;
        Managers.Game.UserInfo.GoogleAccount = googleAccountData;
        
        // 계정 정보가 없는 경우 로그인 화면으로 이동
        if ((string.IsNullOrEmpty(Managers.Game.UserInfo.UserName) || string.IsNullOrEmpty(Managers.Game.UserInfo.Password)) 
            && string.IsNullOrEmpty(Managers.Game.UserInfo.GoogleAccount)) 
        {
            _scene = EScene.SignInScene;
            Managers.Scene.LoadScene(_scene);
            return;
        }

        // 구글 계정이 있으면 구글 계정으로 로그인 시도
        if (!string.IsNullOrEmpty(Managers.Game.UserInfo.GoogleAccount))
        {
            TryLoadUserAccountByGoogle();
        }
        // 일반 계정으로 로그인 시도
        else
        {
            TryLoadUserAccount();
        }
    }

    private void TryLoadUserAccountByGoogle()
    {
        Debug.Log($"구글 계정으로 로그인 시도: {Managers.Game.UserInfo.GoogleAccount}");
        
        Managers.WebContents.GetUserAccountByGoogle(new ReqDtoGetUserAccountByGoogle()
        {
            GoogleAccount = Managers.Game.UserInfo.GoogleAccount
        },
        (response) =>
        {
            _isLoadSceneCondition = true;
            
            // 사용자 정보 설정 (기존 코드와 동일)
            Managers.Game.UserInfo.UserName = response.UserName;
            Managers.Game.UserInfo.UserNickname = response.Nickname;
            Managers.Game.UserInfo.UserAccountId = response.UserAccountId;
            Managers.Game.UserInfo.GoogleAccount = response.GoogleAccount;
            Managers.SignalR.LoginUser(Managers.Game.UserInfo.UserAccountId);

            //캐릭터 스타일 
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
            Debug.Log($"Managers.Game.UserInfo.PlayTime {Managers.Game.UserInfo.PlayTime}");
            Managers.Game.UserInfo.AccumulatedStone = response.AccumulatedStone;
            Managers.Game.UserInfo.StageLevel = response.StageLevel;
            
            // 아이디 저장
            Managers.Login.SaveUserAccountInfo();

            Managers.Event.TriggerEvent(EEventType.OnSettlementComplete);
            Managers.Event.TriggerEvent(EEventType.OnFirstAccept);
        },
        (errorCode) =>
        {
            Debug.LogError($"구글 계정 로그인 실패: {errorCode}");
            _scene = EScene.SignInScene;
            Managers.Scene.LoadScene(_scene);
            return;
        });
        
        StartCoroutine(UpdateEnergy_Co());
    }
    private void TryLoadUserAccount()
    {
        
        Managers.WebContents.GetUserAccount(new ReqDtoGetUserAccount()
        {
            UserName = Managers.Game.UserInfo.UserName,
            Password = Managers.Game.UserInfo.Password
        },
        (response) =>
        {
            _isLoadSceneCondition = true;
            Managers.Game.UserInfo.UserName = response.UserName;
            Managers.Game.UserInfo.UserNickname = response.Nickname;
            Managers.Game.UserInfo.UserAccountId  = response.UserAccountId;
            Managers.Game.UserInfo.GoogleAccount = response.GoogleAccount;
            //Debug.Log($"HandleSuccess Managers.Game.UserInfo.UserAccountId : {Managers.Game.UserInfo.UserAccountId}");
            Managers.SignalR.LoginUser(Managers.Game.UserInfo.UserAccountId);

            //캐릭터 스타일 
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
            Debug.Log($"Managers.Game.UserInfo.PlayTime {Managers.Game.UserInfo.PlayTime}");
            Managers.Game.UserInfo.AccumulatedStone = response.AccumulatedStone;
            Managers.Game.UserInfo.StageLevel = response.StageLevel;

            // 보안 키 저장
            //SecurePlayerPrefs.SetKey(response.SecureKey);
            
            // 아이디 저장
            Managers.Login.SaveUserAccountInfo();

            Managers.Event.TriggerEvent(EEventType.OnSettlementComplete);
            Managers.Event.TriggerEvent(EEventType.OnFirstAccept);
        },
        (errorCode) =>
        {
            _scene = EScene.SignInScene;
            Managers.Scene.LoadScene(_scene);
            return;
        });
        StartCoroutine(UpdateEnergy_Co());
    }
    private IEnumerator UpdateEnergy_Co()
    {
        yield return new WaitWhile(() => _isLoadSceneCondition == false);
        var loadingComplete = UI_LoadingPopup.Show();

        Managers.WebContents.UpdateEnergy(new ReqDtoUpdateEnergy()
        {
            UserAccountId = Managers.Game.UserInfo.UserAccountId
        },
        (response) =>
        {
            loadingComplete.Value = true;
            Debug.Log("OnEvent_UpdateEnergy" + Managers.Game.UserInfo.LatelyEnergy);
            Managers.Game.UserInfo.Energy = response.Energy;
            Managers.Game.UserInfo.LatelyEnergy = response.LatelyEnergy;
            _scene = EScene.SuberunkerSceneHomeScene;
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
            StartCoroutine(LoadUserAccount_Co());
            return;
        }
        _failCount = 0;
        _scene = EScene.StartLoadingScene;
        Managers.Scene.LoadScene(_scene);
    }
    private IEnumerator LoadScene_Co()
    {
        yield return new WaitWhile(() => _isLoadSceneCondition == false);
        Debug.Log("LAST LAST LAST");
        Managers.Scene.LoadScene(_scene);
    }
    private void StartLoadAssets(string label)
    {
        Managers.Resource.LoadAllAsync<UnityEngine.Object>(label, (key, count, totalCount) =>
        {
            _ui.UpdateLogoImage(count / totalCount);
            if (count == totalCount)
            {
                Managers.Data.Init();
                _playableDirector.Play();
                _isPreLoadSuccess = true;
            }
        });
    }
}
