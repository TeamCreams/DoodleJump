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
    private bool _isLoadSceneCondition = false;
    private int _failCount = 0;

    private EErrorCode _errCodeId = EErrorCode.ERR_Nothing;
    private string _password = "";
    //private bool _isFailFirst = false; // for test
    private SignInScene _scene;
    private EScene _loadScene = EScene.Unknown;
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
    public void SetInfo(SignInScene scene)
    {   
        _scene = scene;
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
            var loadingPopup = Managers.UI.ShowPopupUI<UI_LoadingPopup>();

            Managers.UI.ShowPopupUI<UI_ToastPopup>();
            ErrorStruct errorStruct = Managers.Error.GetError(EErrorCode.ERR_NetworkLoginSuccess);
            Managers.Event.TriggerEvent(EEventType.ToastPopupNotice, this, errorStruct.Notice);
            Managers.Game.UserInfo.UserName = GetInputField((int)InputFields.Id_InputField).text;
            Managers.Score.GetScore((this), null,
            () =>
            {
                _loadScene = EScene.SuberunkerSceneHomeScene;
            },
            () =>
            {
                _loadScene = EScene.SignInScene;
            });
            Managers.UI.ClosePopupUI(loadingPopup);
            //StartCoroutine(LoadScene_Co());
            UpdateEnergy();
        }
    }

    private void OnClick_SignUp(PointerEventData eventData)
    {
        _loadScene = EScene.SignUpScene;
        Managers.Scene.LoadScene(_loadScene);
    }

    private IEnumerator LoadScene_Co()
    {
        yield return new WaitWhile(() => _isLoadSceneCondition == false);
        Debug.Log($"Managers.Game.UserInfo.UserAccountId : {Managers.Game.UserInfo.UserAccountId}");
        Managers.Scene.LoadScene(_loadScene);
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
        var loadingPopup = Managers.UI.ShowPopupUI<UI_LoadingPopup>();

        Managers.WebContents.ReqGetUserAccount(new ReqDtoGetUserAccount()
        {
            UserName = GetInputField((int)InputFields.Id_InputField).text,
        },
       (response) =>
       {
           _errCodeId = EErrorCode.ERR_OK;
           _password = response.Password;
           // 유저 정보 저장
           Managers.Game.UserInfo.UserName = response.UserName;
           Managers.Game.UserInfo.UserNickname = response.Nickname;
           Managers.Game.UserInfo.UserAccountId = response.UserAccountId;
            Managers.SignalR.LoginUser(Managers.Game.UserInfo.UserAccountId);

           //캐릭터 스타일 저장
           Managers.Game.ChracterStyleInfo.CharacterId = response.CharacterId;
           Managers.Game.ChracterStyleInfo.Hair = response.HairStyle;
           Managers.Game.ChracterStyleInfo.Eyebrows = response.EyebrowStyle;
           Managers.Game.ChracterStyleInfo.Eyes = response.EyesStyle;
           Managers.Game.UserInfo.EvolutionId = response.Evolution;
           Managers.Game.UserInfo.Energy = response.Energy;
           Managers.Game.UserInfo.LatelyEnergy = response.LatelyEnergy;

           Managers.Event.TriggerEvent(EEventType.OnSettlementComplete);
           Managers.Event.TriggerEvent(EEventType.OnFirstAccept);

           // 아이디 저장
           PlayerPrefs.SetString(HardCoding.UserName, Managers.Game.UserInfo.UserName);
           PlayerPrefs.Save();

           GetInputField((int)InputFields.Password_InputField).enabled = true;
           GetText((int)Texts.Warning_Id_Text).text = "";
           _isLoadSceneCondition = true;
       },
       (errorCode) =>
       {
           _errCodeId = EErrorCode.ERR_ValidationId;
           GetText((int)Texts.Warning_Id_Text).text = _idUnavailable;
       });
        Managers.UI.ClosePopupUI(loadingPopup);
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

    private void UpdateEnergy()
    {
        var loadingPopup = Managers.UI.ShowPopupUI<UI_LoadingPopup>();

        Managers.WebContents.ReqDtoUpdateEnergy(new ReqDtoUpdateEnergy()
        {
            UserAccountId = Managers.Game.UserInfo.UserAccountId
        },
        (response) =>
        {
            Managers.UI.ClosePopupUI(loadingPopup);
            Debug.Log("log in" + Managers.Game.UserInfo.LatelyEnergy);
            Managers.Game.UserInfo.Energy = response.Energy;
            Managers.Game.UserInfo.LatelyEnergy = response.LatelyEnergy;
            StartCoroutine(LoadScene_Co());
        },
        (errorCode) =>
        {
            Managers.UI.ShowPopupUI<UI_ErrorPopup>();
            ErrorStruct errorStruct = Managers.Error.GetError(EErrorCode.ERR_NetworkSaveError);
            Managers.Event.TriggerEvent(EEventType.ToastPopupNotice, this, errorStruct);
            HandleFailure();
        }
        );
    }

    private void HandleFailure()
    {
        if (_failCount < HardCoding.MAX_FAIL_COUNT)
        {
            _failCount++;
            UpdateEnergy();
            return;
        }
        _failCount = 0;
        _loadScene = EScene.SignInScene;
        Managers.Scene.LoadScene(_loadScene);
    }

    void OnEvent_SetLanguage(Component sender, object param)
    {
        GetText((int)Texts.Id_Text).text = Managers.Language.LocalizedString(91027);
        GetText((int)Texts.Placeholder_Id_Text).text = Managers.Language.LocalizedString(91027);
        _idUnavailable = Managers.Language.LocalizedString(91045);

        GetText((int)Texts.Password_Text).text = Managers.Language.LocalizedString(91020);
        GetText((int)Texts.Placeholder_Password_Text).text = Managers.Language.LocalizedString(91020);
        _passwordUnavailable = Managers.Language.LocalizedString(91023);

        GetText((int)Texts.SignUp_Text).text = Managers.Language.LocalizedString(91025);
        GetText((int)Texts.SignIn_Text).text = Managers.Language.LocalizedString(91026);

        _loginSuccess = Managers.Language.LocalizedString(91031);
    }
}
