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
    private EErrorCode _errCodeId;

    private string _idUnavailable = "없는 아이디입니다.";
    private string _passwordUnavailable = "비밀번호가 일치하지 않습니다.";
    private CommonResult<ResDtoGetUserAccount> _rv = null;
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

        GetInputField((int)InputFields.Password_InputField).gameObject.BindEvent(OnClick_CheckLogId, EUIEvent.Click);
        GetText((int)Texts.Warning_Id_Text).text = "";
        GetText((int)Texts.Warning_Password_Text).text = "";
        //Managers.Event.AddEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
        //Managers.Event.TriggerEvent(EEventType.SetLanguage);

        return true;
    }

    private void OnClick_SignIn(PointerEventData eventData)
    {
        EErrorCode errCode = CheckCorrectPassword();
        if (errCode != EErrorCode.ERR_OK)
        {
            return;
        }
        else
        {
            Managers.Scene.LoadScene(EScene.SuberunkerSceneHomeScene);
        }
    }

    private void OnClick_SignUp(PointerEventData eventData)
    {
        Managers.Scene.LoadScene(EScene.SignUpScene);
    }

    private void OnClick_CheckLogId(PointerEventData eventData)
    {
        CheckLogIn(GetInputField((int)InputFields.Id_InputField).text, (result) =>
        {
            _errCodeId = result;
            Debug.Log($"_errCodeId : {_errCodeId}");
        });
    }

    private EErrorCode CheckLogIn(string id, Action<EErrorCode> callback)
    {
        if (string.IsNullOrEmpty(id) || char.IsDigit(id[0]))
        {
            GetText((int)Texts.Warning_Id_Text).text = _idUnavailable;
            return EErrorCode.ERR_ValidationNickname;
        }
        if (16 <  id.Length)
        {
            GetText((int)Texts.Warning_Id_Text).text = _idUnavailable;
            return EErrorCode.ERR_ValidationNickname;
        }

        ReqDtoGetUserAccount requestDto = new ReqDtoGetUserAccount();
        requestDto.UserName = id;
        StartCoroutine(ReturnRv(requestDto, callback));
        return EErrorCode.ERR_ValidationNickname;
    }
    private IEnumerator ReturnRv(ReqDtoGetUserAccount requestDto, Action<EErrorCode> callback)
    {
        _rv = null;
        Managers.Web.SendGetRequest(WebRoute.GetUserAccount(requestDto), (response) =>        
        {
            Debug.Log("Response: " + response);
            _rv = JsonConvert.DeserializeObject<CommonResult<ResDtoGetUserAccount>>(response);
        });

        
        yield return new WaitUntil(() => _rv != null);
        if (_rv.IsSuccess)
        {
            Debug.Log("Available ID");
            GetText((int)Texts.Warning_Id_Text).text = "";
            callback(EErrorCode.ERR_OK);
        }
        else
        {
            Debug.Log("Disabled ID");            
            GetText((int)Texts.Warning_Id_Text).text = _idUnavailable;
            callback(EErrorCode.ERR_ValidationNickname);        
        }
    }

    private EErrorCode CheckCorrectPassword()
    {
        if(_errCodeId != EErrorCode.ERR_OK)
        {
            return EErrorCode.ERR_ValidationNickname;
        }
        if(_rv == null)
        {
            return EErrorCode.ERR_ValidationNickname;
        }
        string password = GetInputField((int)InputFields.Password_InputField).text;
        if (Equals(_rv.Data.Password, password) != true)
        {
            GetText((int)Texts.Warning_Password_Text).text = _passwordUnavailable;
            return EErrorCode.ERR_ValidationNickname;
        }
        GetText((int)Texts.Warning_Password_Text).text = "";
        return EErrorCode.ERR_OK;
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
    }
}
