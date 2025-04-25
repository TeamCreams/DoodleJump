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
        Managers.Login.TryAutoLogin();
        // string usernameData = SecurePlayerPrefs.GetString(HardCoding.UserName, "");
        // string passwordData = SecurePlayerPrefs.GetString(HardCoding.Password, ""); 
        // string googleAccountData = SecurePlayerPrefs.GetString(HardCoding.GoogleAccount, ""); 
        // Managers.Game.UserInfo.UserName = usernameData;
        // Managers.Game.UserInfo.Password = passwordData;
        // Managers.Game.UserInfo.GoogleAccount = googleAccountData;

        // Debug.Log("1--1 usernameData : " + usernameData);

        // Debug.Log("1--1 passwordData : " + passwordData);

        // Debug.Log("1--1 googleAccountData : " + googleAccountData);

        
        // // 계정 정보가 없는 경우 로그인 화면으로 이동
        // if ((string.IsNullOrEmpty(Managers.Game.UserInfo.UserName) || string.IsNullOrEmpty(Managers.Game.UserInfo.Password)) 
        //     && string.IsNullOrEmpty(Managers.Game.UserInfo.GoogleAccount)) 
        // {
        //     _scene = EScene.SignInScene;
        //     Managers.Scene.LoadScene(_scene);
        //     return;
        // }

        // // 구글 계정이 있으면 구글 계정으로 로그인 시도
        // if (!string.IsNullOrEmpty(Managers.Game.UserInfo.GoogleAccount))
        // {
        //     Debug.Log($"구글 계정으로 로그인 시도: {Managers.Game.UserInfo.GoogleAccount}");
        //     Managers.Login.SignInWithGoogle();
        // }
        // // 일반 계정으로 로그인 시도
        // else
        // {
        //     Managers.Login.SignIn(Managers.Game.UserInfo.UserName, Managers.Game.UserInfo.Password);
        // }
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
