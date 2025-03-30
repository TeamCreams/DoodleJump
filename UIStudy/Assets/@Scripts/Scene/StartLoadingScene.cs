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
        // string serializedData = PlayerPrefs.GetString(HardCoding.UserName);
        // 로그인이 이미 되어있는지 확인하려면 계정이 맞는 지 플레이어프리펩스에서 가져옴.
        // 플레이어프리펩스를 열려면 키가 필요.
        // 키는 로그인을 해야 얻을 수 있음.

        // 아이디로 먼저 로그인하고 로그인하면서 받아온 유저정보비번이랑 비교
        string usernameData = SecurePlayerPrefs.GetString(HardCoding.UserName, "");
        string passwordData = SecurePlayerPrefs.GetString(HardCoding.Password, ""); 
        Managers.Game.UserInfo.UserName = usernameData;
        Managers.Game.UserInfo.Password = passwordData;

        if (string.IsNullOrEmpty(Managers.Game.UserInfo.UserName)) 
        {
            _scene = EScene.SignInScene;
            Managers.Scene.LoadScene(_scene);
        }
        // Debug.Log($"UserName : {Managers.Game.UserInfo.UserName}");
        // Debug.Log($"Password : {Managers.Game.UserInfo.Password}");

        TryLoadUserAccount();
    }
    private void TryLoadUserAccount()
    {
        
        Managers.WebContents.ReqGetUserAccount(new ReqDtoGetUserAccount()
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
            SecurePlayerPrefs.SetString(HardCoding.UserName, Managers.Game.UserInfo.UserName);
            SecurePlayerPrefs.SetString(HardCoding.Password, Managers.Game.UserInfo.Password);
            SecurePlayerPrefs.Save();

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

        Managers.WebContents.ReqDtoUpdateEnergy(new ReqDtoUpdateEnergy()
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

    // private void HandleSuccess(ResDtoGetUserAccount response, Action result = null)
    // {        
    //     Managers.Score.GetScore(this, ProcessErrorFun,
    //     () => 
    //     {
    //         _scene = EScene.SuberunkerSceneHomeScene;
    //         result?.Invoke(); 
    //     },
    //     () => 
    //     {
    //         if (_failCount < HardCoding.MAX_FAIL_COUNT)
    //         {                
    //             _failCount++;
    //             return;
    //         }
    //         _failCount = 0;
    //         _scene = EScene.StartLoadingScene;
    //         _isLoadSceneCondition = true;
    //         result?.Invoke();
    //     });
    // }
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
    // public void ProcessErrorFun()
    // {
    //     Managers.Score.GetScore(this);
    // }

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
