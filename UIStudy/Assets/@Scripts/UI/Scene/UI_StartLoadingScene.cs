using System;
using System.Collections;
using GameApi.Dtos;
using UnityEngine;
using UnityEngine.Playables;
using static Define;

public class UI_StartLoadingScene : UI_Scene
{
    private enum Images
    {
        Logo_Image
    }
    private int _failCount = 0;
    private EScene _scene = EScene.InputNicknameScene;
    private bool _isPreLoadSuccess = false;
    private bool _isLoadSceneCondition = false;
    private PlayableDirector _playableDirector = null;

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindImages(typeof(Images));
        _playableDirector = this.gameObject.GetOrAddComponent<PlayableDirector>();
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
        Managers.WebContents.ReqGetOrAddUserAccount(new ReqDtoGetOrAddUserAccount()
        {
            UserName = Managers.Game.UserInfo.UserId,
        },
       (response) =>
       {
            HandleSuccess(response, () => 
            {
                _isLoadSceneCondition = true;
                Managers.Game.UserInfo.UserAccountId = response.UserAccountId;
                //캐릭터 스타일 
                Managers.Game.ChracterStyleInfo.Hair = response.HairStyle;
                Managers.Game.ChracterStyleInfo.Eyebrows = response.EyebrowStyle;
                Managers.Game.ChracterStyleInfo.Eyes = response.EyesStyle;
                Managers.Game.UserInfo.EvolutionId = response.Evolution;
                //
                Managers.Event.TriggerEvent(EEventType.OnSettlementComplete);
                Managers.Event.TriggerEvent(EEventType.OnFirstAccept);
            });
       },
       (errorCode) =>
       {
            Managers.UI.ShowPopupUI<UI_ToastPopup>();

            (string title, string notice) = Managers.Error.GetError(EErrorCode.ERR_NetworkSettlementError);
            Managers.Event.TriggerEvent(EEventType.ToastPopupNotice, this, notice);                 
            Debug.Log($"[Error Code : {errorCode}] error message");
            HandleFailure();
        });
        StartCoroutine(LoadScene_Co());
    }

    private void HandleSuccess(ResDtoGetOrAddUserAccount response, Action result = null)
    {        
        Debug.Log(Managers.Game.UserInfo.UserNickname);

        if (string.IsNullOrEmpty(Managers.Game.UserInfo.UserNickname)) // 최초 로그인
        {
            _scene = EScene.InputNicknameScene;
            result?.Invoke();
            return;
        }
        
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
            GetImage((int)Images.Logo_Image).fillAmount = (float)count / totalCount;
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