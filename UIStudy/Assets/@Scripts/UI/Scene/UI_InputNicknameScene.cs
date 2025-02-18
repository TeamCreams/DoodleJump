using System;
using System.Collections;
using System.Collections.Generic;
using GameApi.Dtos;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_InputNicknameScene : UI_Scene
{
    private enum InputFields
    {
        Nickname_InputField,
    }

    private enum Buttons
    {
        //Next_Button,
        //Prev_Button
        Confirm_Button
    }

    private enum Texts
    {
        Warning_Text,
        Nickname_Text,
        Placeholder_Nickname_Text
    }

    private string _nicknameUnavailable = "사용할 수 없는 닉네임입니다.";

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindInputFields(typeof(InputFields));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        //GetButton((int)Buttons.Prev_Button).gameObject.BindEvent(OnClick_LoginPage, EUIEvent.Click);
        GetButton((int)Buttons.Confirm_Button).gameObject.BindEvent(OnClick_InspectName, EUIEvent.Click);
        GetText((int)Texts.Warning_Text).text = "";

        Managers.Event.AddEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
        OnEvent_SetLanguage(null, null);

        return true;
    }

    private void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
    }
    private void OnClick_LoginPage(PointerEventData eventData)
    {            
        Managers.Scene.LoadScene(EScene.SignInScene);
    }

    private void OnClick_InspectName(PointerEventData eventData)
    {
        EErrorCode errCode = CheckCorrectNickname(GetInputField((int)InputFields.Nickname_InputField).text);
        if (errCode != EErrorCode.ERR_OK)
        {
            //Localization 세계화 번역작업
            //Managers.Data.Localization[][ko]
            GetText((int)Texts.Warning_Text).text = _nicknameUnavailable;
            return;
        }

        //if(eventData.pointerPressRaycast.gameObject != GetInputField((int)InputFields.Nickname_InputField))
        // inspection
        Managers.WebContents.ReqGetValidateUserAccountUserNickName(new ReqDtoGetValidateUserAccountNickname()
        {
            Nickname = GetInputField((int)InputFields.Nickname_InputField).text
        },(response) =>
        {
                Managers.Game.UserInfo.UserNickname = GetInputField((int)InputFields.Nickname_InputField).text;
                InsertUser(() =>
                    Managers.Scene.LoadScene(EScene.SignInScene));        
        },(errorCode) =>
        {
                Managers.UI.ShowPopupUI<UI_ToastPopup>();
                ErrorStruct errorStruct = Managers.Error.GetError(EErrorCode.ERR_ValidationNickname);
                Managers.Event.TriggerEvent(EEventType.ToastPopupNotice, this, errorStruct.Notice);
        });

    }
    private void InsertUser(Action onSuccess = null)
    {
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
            
            onSuccess?.Invoke();
       },
       (errorCode) =>
       {
            Debug.Log("아이디 만들기 실패~");
            Managers.UI.ShowPopupUI<UI_ErrorPopup>();
            ErrorStruct errorStruct = Managers.Error.GetError(EErrorCode.ERR_AccountCreationFailed);
            Managers.Event.TriggerEvent(EEventType.ErrorPopup, this, errorStruct);
       });
    }
    private EErrorCode CheckCorrectNickname(string nickname)
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


    void OnEvent_SetLanguage(Component sender, object param)
    {
        GetText((int)Texts.Nickname_Text).text = Managers.Language.LocalizedString(91014);
        GetText((int)Texts.Placeholder_Nickname_Text).text = Managers.Language.LocalizedString(91014);
        _nicknameUnavailable = Managers.Language.LocalizedString(91015);
    }
}
