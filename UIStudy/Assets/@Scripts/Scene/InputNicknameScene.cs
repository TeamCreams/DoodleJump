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

        var ui = Managers.UI.ShowSceneUI<UI_InputNicknameScene>();
        return true;
    }


    public void InsertUser(Action onSuccess = null)
    {
        ReactiveProperty<bool> loadingComplete = UI_LoadingPopup.Show();

        //var loadingPopup = Managers.UI.ShowPopupUI<UI_LoadingPopup>();
        Managers.WebContents.ReqInsertUserAccount(new ReqDtoInsertUserAccount()
        {
            UserName = Managers.Game.UserInfo.UserName,
            Password = Managers.Game.UserInfo.Password,
            NickName = Managers.Game.UserInfo.UserNickname
        },
       (response) =>
       {
            Debug.Log("아이디 만들기 성공");
            UI_ToastPopup.ShowError(Managers.Error.GetError(EErrorCode.ERR_AccountCreationSuccess));
           //UI_ToastPopup toast = Managers.UI.ShowPopupUI<UI_ToastPopup>();
           //ErrorStruct errorStruct = Managers.Error.GetError(EErrorCode.ERR_AccountCreationSuccess);
           //toast.SetInfo(errorStruct.Notice, UI_ToastPopup.Type.Error);

           loadingComplete.Value = true;
           //Managers.UI.ClosePopupUI(loadingPopup);

            onSuccess?.Invoke();
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
