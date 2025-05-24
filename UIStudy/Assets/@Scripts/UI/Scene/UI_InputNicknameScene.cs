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
        Confirm_Button
    }

    private enum Texts
    {
        Warning_Text,
        Nickname_Text,
        Placeholder_Nickname_Text
    }
    private InputNicknameScene _scene = null;
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


        Managers.Event.AddEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
        OnEvent_SetLanguage(null, null);
        GetText((int)Texts.Warning_Text).text = null;

        GetButton((int)Buttons.Confirm_Button).gameObject.BindEvent(OnClick_InspectName, EUIEvent.Click);

        _scene = Managers.Scene.CurrentScene as InputNicknameScene;

        return true;
    }

    private void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
    }

    private void OnClick_InspectName(PointerEventData eventData)
    {
        EErrorCode errCode = _scene.CheckCorrectNickname(GetInputField((int)InputFields.Nickname_InputField).text);
        if (errCode != EErrorCode.ERR_OK)
        {
            //Localization 세계화 번역작업
            //Managers.Data.Localization[][ko]
            GetText((int)Texts.Warning_Text).text = _nicknameUnavailable;
            return;
        }

        Managers.WebContents.CheckUserAccountNicknameExists(new ReqDtoUserAccountNickname()
        {
            Nickname = GetInputField((int)InputFields.Nickname_InputField).text
        },(response) =>
        {
            Managers.Game.UserInfo.UserNickname = GetInputField((int)InputFields.Nickname_InputField).text;
            _scene.InsertUser();
            //_scene.InsertUser(() => Managers.Scene.LoadScene(EScene.SignInScene));           
        },(errorCode) =>
        {       
            UI_ToastPopup.ShowError(Managers.Error.GetError(EErrorCode.ERR_ValidationNickname));
        });
    }

    void OnEvent_SetLanguage(Component sender, object param)
    {
        GetText((int)Texts.Nickname_Text).text = Managers.Language.LocalizedString(91014);
        GetText((int)Texts.Placeholder_Nickname_Text).text = Managers.Language.LocalizedString(91014);
        _nicknameUnavailable = Managers.Language.LocalizedString(91015);
    }
}
