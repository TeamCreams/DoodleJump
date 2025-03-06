using System;
using System.Collections;
using GameApi.Dtos;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using static Define;

public class StartLoadingScene : BaseScene
{
    /*
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        return true;
    }
*/
    private int _failCount = 0;
    private EScene _scene = EScene.InputNicknameScene;
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
        string serializedData = PlayerPrefs.GetString(HardCoding.UserName);
        Managers.Game.UserInfo.UserName = serializedData;
        //Debug.Log($"UserName : {Managers.Game.UserInfo.UserName}");
        if (string.IsNullOrEmpty(Managers.Game.UserInfo.UserName)) // 최초 로그인
        {
            _scene = EScene.SignInScene;
            Managers.Scene.LoadScene(_scene);
            //_isLoadSceneCondition = true;
        }
        else
        {
            Managers.WebContents.ReqGetUserAccount(new ReqDtoGetUserAccount()
            {
                UserName = Managers.Game.UserInfo.UserName,
            },
        (response) =>
        {
            HandleSuccess(response, () => 
            {
                _isLoadSceneCondition = true;
                Managers.Game.UserInfo.UserName = response.UserName;
                Managers.Game.UserInfo.UserNickname = response.Nickname;
                Managers.Game.UserInfo.UserAccountId  = response.UserAccountId;
                Debug.Log($"HandleSuccess Managers.Game.UserInfo.UserAccountId : {Managers.Game.UserInfo.UserAccountId}");
                Managers.SignalR.LoginUser(Managers.Game.UserInfo.UserAccountId);

                //캐릭터 스타일 
                Managers.Game.ChracterStyleInfo.CharacterId = response.CharacterId;
                Managers.Game.ChracterStyleInfo.Hair = response.HairStyle;
                Managers.Game.ChracterStyleInfo.Eyebrows = response.EyebrowStyle;
                Managers.Game.ChracterStyleInfo.Eyes = response.EyesStyle;
                Managers.Game.UserInfo.EvolutionId = response.Evolution;
                Managers.Game.UserInfo.Energy = response.Energy;
                Managers.Game.UserInfo.LatelyEnergy = response.LatelyEnergy;

                // 아이디 저장
                PlayerPrefs.SetString(HardCoding.UserName, Managers.Game.UserInfo.UserName);
                PlayerPrefs.Save();

                Managers.Event.TriggerEvent(EEventType.OnSettlementComplete);
                Managers.Event.TriggerEvent(EEventType.OnFirstAccept);
            });
        },
        (errorCode) =>
        {
            UI_ToastPopup toast = Managers.UI.ShowPopupUI<UI_ToastPopup>();
            ErrorStruct errorStruct = Managers.Error.GetError(EErrorCode.ERR_NetworkSettlementError);
            toast.SetInfo(errorStruct.Notice, UI_ToastPopup.Type.Error);
    
            Debug.Log($"[Error Code : {errorCode}] error message");
            HandleFailure();
        });
        }
        StartCoroutine(UpdateEnergy_Co());
    }
    private IEnumerator UpdateEnergy_Co()
    {
        yield return new WaitWhile(() => _isLoadSceneCondition == false);
        var loadingPopup = Managers.UI.ShowPopupUI<UI_LoadingPopup>();

        Managers.WebContents.ReqDtoUpdateEnergy(new ReqDtoUpdateEnergy()
        {
            UserAccountId = Managers.Game.UserInfo.UserAccountId
        },
        (response) =>
        {
            Managers.UI.ClosePopupUI(loadingPopup);
            Debug.Log("OnEvent_UpdateEnergy" + Managers.Game.UserInfo.LatelyEnergy);
            Managers.Game.UserInfo.Energy = response.Energy;
            Managers.Game.UserInfo.LatelyEnergy = response.LatelyEnergy;
            StartCoroutine(LoadScene_Co());
        },
        (errorCode) =>
        {
            UI_ToastPopup toast = Managers.UI.ShowPopupUI<UI_ToastPopup>();
            ErrorStruct errorStruct = Managers.Error.GetError(EErrorCode.ERR_NetworkSaveError);
            toast.SetInfo(errorStruct.Notice, UI_ToastPopup.Type.Error);
            HandleFailure();
        }
        );
    }

    private void HandleSuccess(ResDtoGetUserAccount response, Action result = null)
    {        
        Managers.Score.GetScore(this, ProcessErrorFun,
        () => 
        {
            _scene = EScene.SuberunkerSceneHomeScene;
            result?.Invoke(); 
        },
        () => 
        {
            if (_failCount < HardCoding.MAX_FAIL_COUNT)
            {                
                _failCount++;
                return;
            }
            _failCount = 0;
            _scene = EScene.StartLoadingScene;
            _isLoadSceneCondition = true;
            result?.Invoke();
        });
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
    public void ProcessErrorFun()
    {
        Managers.Score.GetScore(this);
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
            _ui.UpdateLogoImage((float)count / totalCount);
            if (count == totalCount)
            {
                Managers.Data.Init();
                _playableDirector.Play();
                _isPreLoadSuccess = true;
             //_playableDirector.stopped += OnPlayableDirectorStopped;   
            }
        });
    }
    
}
