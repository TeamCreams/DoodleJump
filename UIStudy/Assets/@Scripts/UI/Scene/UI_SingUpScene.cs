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

public class UI_SingUpScene : UI_Scene
{

    private enum InputFields
    {
        Id_InputField,
        Password_InputField,
        ConfirmPassword_InputField,  
    }

    private enum Buttons
    {
        Next_Button,
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
        Placeholder_ConfirmPassword_Text
    }

    private string _idUnavailable = "사용할 수 없는 아이디입니다.";
    private string _passwordUnavailable = "20자 이내의 비밀번호를 입력해주세요.";
    private string _confirmPasswordUnavailable = "비밀번호가 일치하지 않습니다.";

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindInputFields(typeof(InputFields));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        GetButton((int)Buttons.Next_Button).gameObject.BindEvent(OnClick_Next, EUIEvent.Click);
        GetText((int)Texts.Warning_Id_Text).text = "";
        GetText((int)Texts.Warning_Password_Text).text = "";
        GetText((int)Texts.Warning_ConfirmPassword_Text).text = "";

        Managers.Event.AddEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
        Managers.Event.TriggerEvent(EEventType.SetLanguage);


        return true;
    }
    private void OnClick_Next(PointerEventData eventData)
    {
        EErrorCode errCode = CheckCorrectId(GetInputField((int)InputFields.Id_InputField).text);

        if (errCode != EErrorCode.ERR_OK)
        {
            GetText((int)Texts.Warning_Id_Text).text = _idUnavailable;
            return;
        }
        Managers.Scene.LoadScene(EScene.SuberunkerTimelineScene);
    }

    private EErrorCode CheckCorrectId(string id)
    {
        ReqDtoGetUserAccount requestDto = new ReqDtoGetUserAccount();
            requestDto.UserName = "test1";
            requestDto.Password = "test1";
            Managers.Web.SendGetRequest(WebRoute.GetUserAccount(requestDto), (response) =>
            {
                CommonResult<ResDtoGetUserAccount> rv = JsonConvert.DeserializeObject<CommonResult<ResDtoGetUserAccount>>(response);

                if(rv.IsSuccess == true)
                {
                    GetText((int)Texts.Warning_Id_Text).text = _idUnavailable;
                }
            });
            
        if (string.IsNullOrEmpty(id) || char.IsDigit(id[0]))
        {
            return EErrorCode.ERR_ValidationNickname;
        }
        if (16 <  id.Length)
        {
            return EErrorCode.ERR_ValidationNickname;
        }

        return EErrorCode.ERR_OK;
    }

    private EErrorCode CheckCorrectPassword(string password)
    {
        if (20 <  password.Length)
        {
            return EErrorCode.ERR_ValidationNickname;
        }
        return EErrorCode.ERR_OK;
    }

    private EErrorCode CheckConfirmPassword(string input)
    {
        string password = GetText((int)Texts.Password_Text).text;
        if (password.CompareTo(input) != 1)
        {
            return EErrorCode.ERR_ValidationNickname;
        }
        return EErrorCode.ERR_OK;
    }

    void OnEvent_SetLanguage(Component sender, object param)
    {
        GetText((int)Texts.Id_Text).text = Managers.Language.LocalizedString(91013);
        GetText((int)Texts.Placeholder_Id_Text).text = Managers.Language.LocalizedString(91014);
        _idUnavailable = Managers.Language.LocalizedString(91015);

        GetText((int)Texts.Password_Text).text = Managers.Language.LocalizedString(91020);
        GetText((int)Texts.Placeholder_Password_Text).text = Managers.Language.LocalizedString(91020);
        _passwordUnavailable = Managers.Language.LocalizedString(91021);

        GetText((int)Texts.ConfirmPassword_Text).text = Managers.Language.LocalizedString(91022);
        GetText((int)Texts.Placeholder_ConfirmPassword_Text).text = Managers.Language.LocalizedString(91022);
        _confirmPasswordUnavailable = Managers.Language.LocalizedString(91023);
    }
}