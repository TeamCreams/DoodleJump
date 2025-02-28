using System;
using System.Collections;
using System.Collections.Generic;
using GameApi.Dtos;
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
        ui.SetInfo(this);
        return true;
    }


    public void InsertUser(Action onSuccess = null)
    {
        var loadingPopup = Managers.UI.ShowPopupUI<UI_LoadingPopup>();
        Managers.WebContents.ReqInsertUserAccount(new ReqDtoInsertUserAccount()
        {
            UserName = Managers.Game.UserInfo.UserName,
            Password = Managers.Game.UserInfo.Password,
            NickName = Managers.Game.UserInfo.UserNickname
        },
       (response) =>
       {
            Debug.Log("아이디 만들기 성공");
            Managers.UI.ShowPopupUI<UI_ToastPopup>();
            ErrorStruct errorStruct = Managers.Error.GetError(EErrorCode.ERR_AccountCreationSuccess);
            Managers.Event.TriggerEvent(EEventType.ToastPopupNotice, this, errorStruct.Notice);
            Managers.UI.ClosePopupUI(loadingPopup);

            onSuccess?.Invoke();
       },
       (errorCode) =>
       {            
            Managers.UI.ClosePopupUI(loadingPopup);

            Debug.Log("아이디 만들기 실패~");
            Managers.UI.ShowPopupUI<UI_ErrorPopup>();
            ErrorStruct errorStruct = Managers.Error.GetError(EErrorCode.ERR_AccountCreationFailed);
            Managers.Event.TriggerEvent(EEventType.ErrorPopup, this, errorStruct);
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
