using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;
using GameApi.Dtos;
using Newtonsoft.Json;
using System.Net.Http;
using System;
using WebApi.Models.Dto;
using System.Collections;

public class UI_SignInScene : UI_Scene
{

    private enum InputFields
    {
        Id_InputField,
        Password_InputField,
    }

    private enum Buttons
    {
        SignUp_Button,
        SignIn_Button
    }

    private enum Texts
    {
        Warning_Id_Text,
        Id_Text,
        Placeholder_Id_Text,
        Warning_Password_Text,
        Password_Text,
        Placeholder_Password_Text,
        SignIn_Text,
        SignUp_Text
    }

    private string _idUnavailable = "없는 아이디입니다.";
    private string _passwordUnavailable = "비밀번호가 일치하지 않습니다.";    
    private string _loginSuccess = "Login successful.";

    private EErrorCode _errCodeId = EErrorCode.ERR_Nothing;
    private string _password = "";
    //private bool _isFailFirst = false; // for test

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        BindInputFields(typeof(InputFields));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        GetButton((int)Buttons.SignIn_Button).gameObject.BindEvent(OnClick_SignIn, EUIEvent.Click);
        GetButton((int)Buttons.SignUp_Button).gameObject.BindEvent(OnClick_SignUp, EUIEvent.Click);

        GetInputField((int)InputFields.Password_InputField).enabled = false;
        GetInputField((int)InputFields.Password_InputField).gameObject.BindEvent(OnClick_CheckLogId, EUIEvent.Click);
        GetText((int)Texts.Warning_Id_Text).text = "";
        GetText((int)Texts.Warning_Password_Text).text = "";
        Managers.Event.AddEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
        OnEvent_SetLanguage(null, null);

        return true;
    }
    private void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
    }
    private void OnClick_SignIn(PointerEventData eventData)
    {
        //최종 로그인 하는 버튼.
        if (_errCodeId != EErrorCode.ERR_OK)
        {
            Managers.UI.ShowPopupUI<UI_ToastPopup>();        
            ErrorStruct errorStruct = Managers.Error.GetError(EErrorCode.ERR_NetworkIDError);
            Managers.Event.TriggerEvent(EEventType.ToastPopupNotice, this, errorStruct.Notice);
            return;
        }

        EErrorCode error = CheckCorrectPassword();
        if (error != EErrorCode.ERR_OK)
        {
            Managers.UI.ShowPopupUI<UI_ToastPopup>();
            Managers.Event.TriggerEvent(EEventType.ToastPopupNotice, this, _passwordUnavailable);
            return;
        }

        //1. 다른버튼 비활성화
        //2. 로딩 인디케이터
        {
            Managers.UI.ShowPopupUI<UI_ToastPopup>();
            ErrorStruct errorStruct = Managers.Error.GetError(EErrorCode.ERR_NetworkLoginSuccess);
            Managers.Event.TriggerEvent(EEventType.ToastPopupNotice, this, errorStruct.Notice);
            Managers.Game.UserInfo.UserId = GetInputField((int)InputFields.Id_InputField).text;
            Managers.Score.GetScore((this), null,
            () => 
            {
                if(string.IsNullOrEmpty(Managers.Game.UserInfo.UserNickname))
                {
                    Debug.Log("InputNicknameScene");
                    Managers.Scene.LoadScene(EScene.InputNicknameScene);
                    return;
                }
                Managers.Scene.LoadScene(EScene.SuberunkerSceneHomeScene);
            },
            () => 
            {
/*
                Debug.Log("is SignInScene");
                if(_isFailFirst == true)
                {                
                    Debug.Log("is second");
                    _isFailFirst = false;
                    Managers.Scene.LoadScene(EScene.SignInScene);
                    return;
                }
                Debug.Log("is first");
                _isFailFirst = true;
                Managers.Score.GetScore(this);
*/
                Managers.Scene.LoadScene(EScene.SignInScene);
            }
            );
        }
    }

    private void OnClick_SignUp(PointerEventData eventData)
    {
        Managers.Scene.LoadScene(EScene.SignUpScene);
    }

    /// <summary>
    /// 패스워드 Input Field 를 눌렀을떄 호출
    /// </summary>
    /// <param name="eventData"></param>
    private void OnClick_CheckLogId(PointerEventData eventData)
    {
        // 1. 비밀번호 인풋필드를 누른다.
        // 2. 아이디를 조회한다.
        //      - 있음 -> 로그인 가능
        //      - 없음 -> 불가.

        Managers.WebContents.ReqGetUserAccount(new ReqDtoGetUserAccount()
        {
            UserName = GetInputField((int)InputFields.Id_InputField).text,
        },
       (response) =>
       {
            _errCodeId = EErrorCode.ERR_OK;
            _password = response.Password;
            Managers.Game.UserInfo.UserNickname = response.Nickname;
            GetInputField((int)InputFields.Password_InputField).enabled = true;
            GetText((int)Texts.Warning_Id_Text).text = "";
       },
       (errorCode) =>
       {
           _errCodeId = EErrorCode.ERR_ValidationId;
           GetText((int)Texts.Warning_Id_Text).text = _idUnavailable;
       });
    }

    private EErrorCode CheckCorrectPassword()
    {
        string password = GetInputField((int)InputFields.Password_InputField).text;

        if (Equals(_password, password) != true)
        {
            GetText((int)Texts.Warning_Password_Text).text = _passwordUnavailable;
            return EErrorCode.ERR_ValidationPassword;
        }
        else
        {
            GetText((int)Texts.Warning_Password_Text).text = "";
            return EErrorCode.ERR_OK;
        }
    }

    void OnEvent_SetLanguage(Component sender, object param)
    {
        GetText((int)Texts.Id_Text).text = Managers.Language.LocalizedString(91027);
        GetText((int)Texts.Placeholder_Id_Text).text = Managers.Language.LocalizedString(91027);
        _idUnavailable = Managers.Language.LocalizedString(91024);

        GetText((int)Texts.Password_Text).text = Managers.Language.LocalizedString(91020);
        GetText((int)Texts.Placeholder_Password_Text).text = Managers.Language.LocalizedString(91020);
        _passwordUnavailable = Managers.Language.LocalizedString(91023);

        GetText((int)Texts.SignUp_Text).text = Managers.Language.LocalizedString(91025);
        GetText((int)Texts.SignIn_Text).text = Managers.Language.LocalizedString(91026);

        _loginSuccess = Managers.Language.LocalizedString(91031);
    }
}
