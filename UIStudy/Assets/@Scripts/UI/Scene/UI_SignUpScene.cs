using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;
using GameApi.Dtos;
using Newtonsoft.Json;
using System.Net.Http;
using System;
using WebApi.Models.Dto;

public class UI_SignUpScene : UI_Scene
{

    private enum InputFields
    {
        Id_InputField,
        Password_InputField,
        ConfirmPassword_InputField,  
    }

    private enum Buttons
    {
        DuplicateIdCheck_Button,
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
        Warning_ConfirmPassword_Text,
        ConfirmPassword_Text,
        Placeholder_ConfirmPassword_Text,
        SignIn_Text,
    }

    private string _idUnavailable = "사용할 수 없는 아이디입니다.";
    private string _passwordUnavailable = "20자 이내의 비밀번호를 입력해주세요.";
    private string _confirmPasswordUnavailable = "비밀번호가 일치하지 않습니다.";

    private EErrorCode _errCodeId = EErrorCode.ERR_Nothing;
    private EErrorCode _errCodePassword = EErrorCode.ERR_ValidationPassword;

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        BindInputFields(typeof(InputFields));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        GetButton((int)Buttons.DuplicateIdCheck_Button).gameObject.BindEvent(OnClick_DuplicateIdCheck, EUIEvent.Click);
        GetButton((int)Buttons.SignIn_Button).gameObject.BindEvent(OnClick_SignIn, EUIEvent.Click);

        GetInputField((int)InputFields.Id_InputField).gameObject.BindEvent(OnClick_InputId, EUIEvent.Click);
        GetInputField((int)InputFields.Password_InputField).gameObject.BindEvent(OnClick_IsCheckCorrectId, EUIEvent.Click);
        GetInputField((int)InputFields.ConfirmPassword_InputField).gameObject.BindEvent(OnClick_CheckCorrectPassword, EUIEvent.Click);

        GetInputField((int)InputFields.Password_InputField).enabled = false;
        GetInputField((int)InputFields.ConfirmPassword_InputField).enabled = false;

        GetText((int)Texts.Warning_Id_Text).text = "";
        GetText((int)Texts.Warning_Password_Text).text = "";
        GetText((int)Texts.Warning_ConfirmPassword_Text).text = "";
        Managers.Event.AddEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
        Managers.Event.TriggerEvent(EEventType.SetLanguage);

        return true;
    }
    private void OnClick_DuplicateIdCheck(PointerEventData eventData)
    {
        CheckCorrectId(GetInputField((int)InputFields.Id_InputField).text);
    }

    private void OnClick_InputId(PointerEventData eventData)
    {
        if(_errCodeId == EErrorCode.ERR_OK)
        {
            return;
        }
        _errCodeId = EErrorCode.ERR_Nothing;
    }
    private void OnClick_IsCheckCorrectId(PointerEventData eventData)
    {
        if(_errCodeId == EErrorCode.ERR_Nothing)
        {
            GetText((int)Texts.Warning_Id_Text).text = "Please check for username availability.";
            return;
        }
        GetText((int)Texts.Warning_Id_Text).text = "";
    }

    private void OnClick_CheckCorrectPassword(PointerEventData eventData)
    {
        _errCodePassword = CheckCorrectPassword(GetInputField((int)InputFields.Password_InputField).text);
    }

    private void OnClick_SignIn(PointerEventData eventData)
    {
        EErrorCode errCode = CheckConfirmPassword(GetInputField((int)InputFields.Password_InputField).text);
        if (string.IsNullOrEmpty(GetInputField((int)InputFields.Id_InputField).text))
        {
            Debug.Log("아이디 안 만들고 그냥 넘어감");
            Managers.Scene.LoadScene(EScene.SignInScene);
            return; 
        }
        if (errCode != EErrorCode.ERR_OK || _errCodeId != EErrorCode.ERR_OK)
        {
            UI_ErrorButtonPopup popup = Managers.UI.ShowPopupUI<UI_ErrorButtonPopup>();
            Managers.Event.TriggerEvent(EEventType.ErrorButtonPopup, this, "Do you want to cancel account creation?");
            popup.AddOnClickAction(ProcessErrorFun);
            // 아이디 생성 안 된다고 말하고 가만히 있기/로그인창으로넘어가기 선택 팝업.
            return; 
        }
        
        InsertUser(() =>
           Managers.Scene.LoadScene(EScene.SignInScene));
    }
    
    private void InsertUser(Action onSuccess = null)
    {
        Managers.WebContents.ReqInsertUserAccount(new ReqDtoInsertUserAccount()
        {
            UserName = GetInputField((int)InputFields.Id_InputField).text,
            Password = GetInputField((int)InputFields.Password_InputField).text
        },
       (response) =>
       {
            Debug.Log("아이디 만들기 성공");
            Managers.UI.ShowPopupUI<UI_ToastPopup>();
            Managers.Event.TriggerEvent(EEventType.ToastPopupNotice, this, "Account creation successful.");
            onSuccess?.Invoke();
       },
       (errorCode) =>
       {
            Debug.Log("아이디 만들기 실패~");
            Managers.UI.ShowPopupUI<UI_ErrorPopup>();
            Managers.Event.TriggerEvent(EEventType.ErrorPopup,
             this, 
            ("Failed to create account.", "Account creation has failed.\n Please try again."));
       });
    }

    private void CheckCorrectId(string id)
    {
        if (string.IsNullOrEmpty(id) || char.IsDigit(id[0]))
        {
            GetText((int)Texts.Warning_Id_Text).text = _idUnavailable;
            _errCodeId =  EErrorCode.ERR_ValidationId;
        }
        if (16 <  id.Length)
        {
            GetText((int)Texts.Warning_Id_Text).text = _idUnavailable;
            _errCodeId = EErrorCode.ERR_ValidationId;
        }

        Managers.WebContents.ReqGetValidateUserAccountId(new ReqDtoGetValidateUserAccountId()
        {
            UserName = GetInputField((int)InputFields.Id_InputField).text,
        },
       (response) =>
       {
           GetText((int)Texts.Warning_Id_Text).text = "";
           _errCodeId = EErrorCode.ERR_OK;
            GetInputField((int)InputFields.Password_InputField).enabled = true;
       },
       (errorCode) =>
       {
           GetText((int)Texts.Warning_Id_Text).text = _idUnavailable;
           _errCodeId = EErrorCode.ERR_DuplicateId;
       });
    }

    private EErrorCode CheckCorrectPassword(string password)
    {
        if (password.Length < 8 || 20 <  password.Length)
        {
            GetText((int)Texts.Warning_Password_Text).text = _passwordUnavailable;
            return EErrorCode.ERR_ValidationPassword;
        }
        GetText((int)Texts.Warning_Password_Text).text = "";
        GetInputField((int)InputFields.ConfirmPassword_InputField).enabled = true;
        return EErrorCode.ERR_OK;
    }

    private EErrorCode CheckConfirmPassword(string input)
    {
        GetText((int)Texts.Warning_Id_Text).text = "";
        string confirmPassword = GetInputField((int)InputFields.ConfirmPassword_InputField).text;
        if (String.Equals(input, confirmPassword) != true || _errCodePassword != EErrorCode.ERR_OK)
        {
            GetText((int)Texts.Warning_ConfirmPassword_Text).text = _confirmPasswordUnavailable;
            return EErrorCode.ERR_ConfirmPassword;
        }
        GetText((int)Texts.Warning_ConfirmPassword_Text).text = "";
        return EErrorCode.ERR_OK;
    }

    public void ProcessErrorFun()
    {
    }

    void OnEvent_SetLanguage(Component sender, object param)
    {
        GetText((int)Texts.Id_Text).text = Managers.Language.LocalizedString(91027);
        GetText((int)Texts.Placeholder_Id_Text).text = Managers.Language.LocalizedString(91027);
        _idUnavailable = Managers.Language.LocalizedString(91024);

        GetText((int)Texts.Password_Text).text = Managers.Language.LocalizedString(91020);
        GetText((int)Texts.Placeholder_Password_Text).text = Managers.Language.LocalizedString(91020);
        _passwordUnavailable = Managers.Language.LocalizedString(91021);

        GetText((int)Texts.ConfirmPassword_Text).text = Managers.Language.LocalizedString(91022);
        GetText((int)Texts.Placeholder_ConfirmPassword_Text).text = Managers.Language.LocalizedString(91022);
        _confirmPasswordUnavailable = Managers.Language.LocalizedString(91023);
        
        GetText((int)Texts.SignIn_Text).text = Managers.Language.LocalizedString(91026);
    }
}
