using System;
using System.Collections;
using System.Collections.Generic;
using GameApi.Dtos;
using UniRx;
using UnityEngine;
using static Define;

public class InputNicknameScene : BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        Managers.UI.ShowSceneUI<UI_InputNicknameScene>();
        return true;
    }

    public void InsertUser(Action onSuccess = null)
    {
        Debug.Log($"GoogleAccount : {Managers.Game.UserInfo.GoogleAccount}");
        if(Managers.Game.UserInfo.GoogleAccount == HardCoding.GoogleAccount)
        {
            WithUserAccount();
        }
        else
        {
            WithGoogleAccount();
        }
    }

    public void WithUserAccount()//Action onSuccess = null
    {
        ReactiveProperty<bool> loadingComplete = UI_LoadingPopup.Show();

        Managers.WebContents.InsertUserAccount(new ReqDtoInsertUserAccount()
        {
            UserName = Managers.Game.UserInfo.UserName,
            Password = Managers.Game.UserInfo.Password,
            NickName = Managers.Game.UserInfo.UserNickname
        },
       (response) =>
       {
            Debug.Log("아이디 만들기 성공");

            loadingComplete.Value = true;
            UI_ToastPopup.ShowError(Managers.Error.GetError(EErrorCode.ERR_AccountCreationSuccess), 2f,
            () => Managers.Scene.LoadScene(EScene.SignInScene));

            //onSuccess?.Invoke();
       },
       (errorCode) =>
       {
           loadingComplete.Value = true;

            Debug.Log("아이디 만들기 실패~");
            UI_ErrorPopup.ShowError(Managers.Error.GetError(EErrorCode.ERR_AccountCreationFailed));
       });
    }

    public void WithGoogleAccount()//Action onSuccess = null
    {
        ReactiveProperty<bool> loadingComplete = UI_LoadingPopup.Show();

        Managers.WebContents.InsertGoogleAccount(new ReqDtoInsertGoogleAccount()
        {
            GoogleAccount = Managers.Game.UserInfo.GoogleAccount,
            NickName = Managers.Game.UserInfo.UserNickname
        },
       (response) =>
       {
            Debug.Log("아이디 만들기 성공");
            loadingComplete.Value = true;

            UI_ToastPopup.ShowError(Managers.Error.GetError(EErrorCode.ERR_AccountCreationSuccess), 2f,
            () => Managers.Scene.LoadScene(EScene.SignInScene));
            //onSuccess?.Invoke();
       },
       (errorCode) =>
       {
           loadingComplete.Value = true;

            Debug.Log("아이디 만들기 실패~");
            UI_ErrorPopup.ShowError(Managers.Error.GetError(EErrorCode.ERR_AccountCreationFailed));
       });
    }
    public EErrorCode CheckCorrectNickname(string nickname)
    {
        if (string.IsNullOrEmpty(nickname))
        {
            return EErrorCode.ERR_ValidationNickname;
        }
        if (20 <  nickname.Length)
        {
            return EErrorCode.ERR_ValidationNickname;
        }
        return EErrorCode.ERR_OK;
    }
}
