using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using GameApi.Dtos;
using UnityEngine;
using static Define;

public class UI_StartLoadingScene : UI_Scene
{
    private enum GameObjects
    {

    }

    private int _failCount = 0;
    private EScene _scene = EScene.InputNicknameScene;

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        _failCount = 0;
        StartLoadAssets("PreLoad");

        return true;
    }

    public void OnEvent_LoadUserAccount()
    {
        Managers.WebContents.ReqGetOrAddUserAccount(null,
       (response) =>
       {
                Debug.Log("OnEvent_LoadUserAccount");

            HandleSuccess((response),
                ()=>
                {
                    Debug.Log("loadingScene");
                    Managers.Scene.LoadScene(_scene);
                }
            );
       },
       (errorCode) =>
       {
            Managers.UI.ShowPopupUI<UI_ToastPopup>();
            Managers.Event.TriggerEvent(EEventType.ToastPopupNotice, this, "The settlement could not be processed due to poor network conditions.");
            if(_failCount < HardCoding.MAX_FAIL_COUNT)
            {                
                _failCount++;
                StartCoroutine(LoadUserAccount_Co());
                return;
            }
            _failCount = 0;
            _scene = EScene.StartLoadingScene;
            Managers.Scene.LoadScene(_scene);
        });
    }

    IEnumerator LoadUserAccount_Co()
    {
        yield return new WaitForSeconds(0.5f);
        OnEvent_LoadUserAccount();
    }

    private void HandleSuccess(ResDtoGetOrAddUserAccount response, Action result = null)
    {        
        Debug.Log(Managers.Game.UserInfo.UserNickname);
        Debug.Log("HandleSuccess");

        if(string.IsNullOrEmpty(Managers.Game.UserInfo.UserNickname)) //최초 로그인이라는 뜻
        {
            _scene = EScene.InputNicknameScene;
            result?.Invoke();
            return;
        }
        
        Managers.Score.GetScore((this), ProcessErrorFun,
        () => 
        {
            _scene = EScene.SuberunkerSceneHomeScene;    
            result?.Invoke(); 
        },
        () => 
        {
            if(_failCount < HardCoding.MAX_FAIL_COUNT)
            {                
                _failCount++;
                return;
            }
            _failCount = 0;
            _scene = EScene.StartLoadingScene;
            result?.Invoke();
        }
        );   
    }
    public void ProcessErrorFun()
    {
        Managers.Score.GetScore(this);
    }

    void StartLoadAssets(string label)
	{
		Managers.Resource.LoadAllAsync<UnityEngine.Object>(label, (key, count, totalCount) =>
		{
			Debug.Log($"{key} {count}/{totalCount}");

			if (count == totalCount)
			{
				//Debug.Log("Load Complete");
				Managers.Data.Init();
			}
		});
	}

}
