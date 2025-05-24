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

    private enum Images
    {
        DuplicateIdCheck_Button,
        SignIn_Button,
        GoogleSignup_Button
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
    private string _enterValidId = "유효한 아이디를 입력하세요.";

    private EErrorCode _errCodeId = EErrorCode.ERR_Nothing;
    private EErrorCode _errCodePassword = EErrorCode.ERR_ValidationPassword;
    private SignUpScene _scene;

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        BindInputFields(typeof(InputFields));
        BindImages(typeof(Images));
        BindTexts(typeof(Texts));

        GetImage((int)Images.DuplicateIdCheck_Button).gameObject.BindEvent(OnClick_DuplicateIdCheck, EUIEvent.Click);
        GetImage((int)Images.SignIn_Button).gameObject.BindEvent(OnClick_SignIn, EUIEvent.Click);
        GetImage((int)Images.GoogleSignup_Button).gameObject.BindEvent(OnClick_GoogleSignup, EUIEvent.Click);

        GetInputField((int)InputFields.Id_InputField).gameObject.BindEvent(OnClick_InputId, EUIEvent.Click);
        GetInputField((int)InputFields.Password_InputField).gameObject.BindEvent(OnClick_IsCheckCorrectId, EUIEvent.Click);
        GetInputField((int)InputFields.ConfirmPassword_InputField).gameObject.BindEvent(OnClick_CheckCorrectPassword, EUIEvent.Click);

        GetInputField((int)InputFields.Password_InputField).enabled = false;
        GetInputField((int)InputFields.ConfirmPassword_InputField).enabled = false;

        Managers.Event.AddEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
        OnEvent_SetLanguage(null, null);

        GetText((int)Texts.Warning_Id_Text).text = null;
        GetText((int)Texts.Warning_Password_Text).text = null;
        GetText((int)Texts.Warning_ConfirmPassword_Text).text = null;

        _scene = Managers.Scene.CurrentScene as SignUpScene;

        return true;
    }
    private void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
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
            GetText((int)Texts.Warning_Id_Text).text = _enterValidId;
            return;
        }
        GetText((int)Texts.Warning_Id_Text).text = "";
    }

    private void OnClick_CheckCorrectPassword(PointerEventData eventData)
    {
        _errCodePassword = CheckCorrectPassword(GetInputField((int)InputFields.Password_InputField).text);
    }

    private void OnClick_GoogleSignup(PointerEventData eventData)
    {
        if(Managers.Game.UserInfo.GoogleAccount == HardCoding.GoogleAccount || string.IsNullOrEmpty(Managers.Game.UserInfo.GoogleAccount))
        {
            Systems.GoogleLoginWebView.SignIn();
        }
        else
        {
            _scene.GoogleAccountSignup(Managers.Game.UserInfo.GoogleAccount);
        }
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
            UI_ErrorButtonPopup.ShowErrorButton(Managers.Error.GetError(EErrorCode.ERR_AccountCreationCancellation)); // 아이디 생성 안 된다고 말하고 가만히 있기/로그인창으로넘어가기 선택 팝업.
            return; 
        }        
        Managers.Scene.LoadScene(EScene.InputNicknameScene);
    }
    
    private void CheckCorrectId(string id)
    {
        if (string.IsNullOrEmpty(id) || char.IsDigit(id[0]))
        {
            GetText((int)Texts.Warning_Id_Text).text = _idUnavailable;
            _errCodeId =  EErrorCode.ERR_ValidationId;
        }
        if (16 < id.Length)
        {
            GetText((int)Texts.Warning_Id_Text).text = _idUnavailable;
            _errCodeId = EErrorCode.ERR_ValidationId;
        }

        var loadingComplete = UI_LoadingPopup.Show();

        Managers.WebContents.CheckUserAccountUserNameExists(new ReqDtoUserAccountUserName()
        {
            UserName = GetInputField((int)InputFields.Id_InputField).text,
        },
       (response) =>
       {
           GetText((int)Texts.Warning_Id_Text).text = "";
           Managers.Game.UserInfo.UserName = id;
           _errCodeId = EErrorCode.ERR_OK;
            GetInputField((int)InputFields.Password_InputField).enabled = true;
       },
       (errorCode) =>
       {
           GetText((int)Texts.Warning_Id_Text).text = _idUnavailable;
           _errCodeId = EErrorCode.ERR_ValidationId;
       });
        loadingComplete.Value = true;
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
        if (Equals(input, confirmPassword) != true || _errCodePassword != EErrorCode.ERR_OK)
        {
            GetText((int)Texts.Warning_ConfirmPassword_Text).text = _confirmPasswordUnavailable;
            return EErrorCode.ERR_ConfirmPassword;
        }
        GetText((int)Texts.Warning_ConfirmPassword_Text).text = "";
        Managers.Game.UserInfo.Password = input;
        return EErrorCode.ERR_OK;
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
        _enterValidId = Managers.Language.LocalizedString(91032);
    }
}
