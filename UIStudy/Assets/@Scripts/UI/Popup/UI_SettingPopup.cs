using Data;
using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public class UI_SettingPopup : UI_Popup
{
    private enum Texts
    {
        Setting_Text,
        Music_Text,
        SoundFx_Text,
        Vibration_Text,
        Language_Text,
        Logout_Text,
        Google_Text,
    }

    private enum Buttons
    {
        Close_Button
    }
    private enum Sliders
    {
        Music_Slider,
        SoundFx_Slider
    }
    private enum Toggles
    {
        Language_En,
        Language_Kr
    }
    private BaseScene _scene;

    private PersonalSetting _personalSettingData = new PersonalSetting();
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindTexts(typeof(Texts));
        BindButtons(typeof(Buttons));
        BindSliders(typeof(Sliders));
        BindToggles(typeof(Toggles));
        
        Managers.Event.AddEvent(EEventType.SetLanguage, OnEvent_SetLanguage);

        GetButton((int)Buttons.Close_Button).gameObject.BindEvent(OnClick_CloseButton, EUIEvent.Click);
        GetToggle((int)Toggles.Language_En).gameObject.BindEvent(OnClick_SetLanguage, EUIEvent.Click);
        GetToggle((int)Toggles.Language_Kr).gameObject.BindEvent(OnClick_SetLanguage, EUIEvent.Click);
        GetSlider((int)Sliders.Music_Slider).gameObject.BindEvent(OnDrag_MusicSlider, EUIEvent.Drag);
        GetSlider((int)Sliders.SoundFx_Slider).gameObject.BindEvent(OnDrag_SoundFxSlider, EUIEvent.Drag);
        GetText((int)Texts.Logout_Text).gameObject.BindEvent(OnClick_Logout, EUIEvent.Click);
        GetText((int)Texts.Google_Text).gameObject.BindEvent(OnClick_Google, EUIEvent.Click);
        return true;
    }
    private void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
    }

    public void ActiveInfo()
    {
        string serializedData = PlayerPrefs.GetString(HardCoding.PersonlSetting);
        _personalSettingData = PersonalSetting.Deserialize(serializedData);

        OnEvent_SetLanguage(null, null);
        GetSlider((int)Sliders.Music_Slider).value = _personalSettingData.MusicVolume;
        GetSlider((int)Sliders.SoundFx_Slider).value = _personalSettingData.SoundFxVolume;
        Managers.Game.SettingInfo.VibrationIsOn = _personalSettingData.IsOnVibration;

        if(_personalSettingData.IsOnKr)
        {
            GetToggle((int)Toggles.Language_Kr).isOn = _personalSettingData.IsOnKr;
        }
        else
        {
            GetToggle((int)Toggles.Language_En).isOn = _personalSettingData.IsOnKr;
        }
    }

    private void OnClick_CloseButton(PointerEventData eventData)
    {
        // save json file 
        string serializedData = _personalSettingData.Serialize();
        PlayerPrefs.SetString(HardCoding.PersonlSetting, serializedData);
        PlayerPrefs.Save();

        _personalSettingData.IsOnVibration = Managers.Game.SettingInfo.VibrationIsOn;
        Systems.GoogleLoginWebView.OnGetGoogleAccount -= GoogleAccountSignin; // 구독 해제
        Managers.UI.ClosePopupUI(this);
    }

    private void OnClick_SetLanguage(PointerEventData eventData)
    {
        if(GetToggle((int)Toggles.Language_Kr).isOn == true)
        {
            Managers.Language.ELanguageInfo = ELanguage.Kr;
            _personalSettingData.IsOnKr = true;
        }
        else
        {
            Managers.Language.ELanguageInfo = ELanguage.En;
            _personalSettingData.IsOnKr = false;
        }
        Managers.Event.TriggerEvent(EEventType.SetLanguage);
        Debug.Log($"language : {Managers.Language.ELanguageInfo}");
    }

    private void OnDrag_MusicSlider(PointerEventData eventData)
    {
        _personalSettingData.MusicVolume = GetSlider((int)Sliders.Music_Slider).value;
        Managers.Sound.SoundValue = _personalSettingData.MusicVolume;
    }

    private void OnDrag_SoundFxSlider(PointerEventData eventData)
    {
        _personalSettingData.SoundFxVolume = GetSlider((int)Sliders.SoundFx_Slider).value;
        Managers.Sound.SoundFxValue = _personalSettingData.SoundFxVolume;
    }
    private void OnClick_Logout(PointerEventData eventData)
    {
        // 창띄우기
        UI_ErrorButtonPopup.ShowErrorButton(Managers.Error.GetError(EErrorCode.ERR_Logout), LogoutAndClearMission);
        // ErrorStruct errorStruct = Managers.Error.GetError(EErrorCode.ERR_Logout);
        // popup.SetInfo(errorStruct.Notice, LogoutAndClearMission);

    }

    private void OnClick_Google(PointerEventData eventData)
    {
        UI_ErrorButtonPopup.ShowErrorButton(Managers.Error.GetError(EErrorCode.ERR_GoogleAccountMergeConfirm),// 메세지를 바꿔야함.         
        () => {
            Systems.GoogleLoginWebView.OnGetGoogleAccount -= GoogleAccountSignin; // 구독 해제
            Systems.GoogleLoginWebView.OnGetGoogleAccount += GoogleAccountSignin; // 이벤트 구독
            Systems.GoogleLoginWebView.SignIn();
        });
    }

    public void GoogleAccountSignin(string googleAccount)
    {
        Managers.Login.ProcessGoogleAccount(googleAccount, true);
    }
    private void LogoutAndClearMission()
    {
        Managers.Login.LogoutAndClearMission();
    }
    void OnEvent_SetLanguage(Component sender, object param)
    {
        GetText((int)Texts.Setting_Text).text = Managers.Language.LocalizedString(91037);
        GetText((int)Texts.Music_Text).text = Managers.Language.LocalizedString(91038);
        GetText((int)Texts.SoundFx_Text).text = Managers.Language.LocalizedString(91039);
        GetText((int)Texts.Vibration_Text).text = Managers.Language.LocalizedString(91040);
        GetText((int)Texts.Language_Text).text = Managers.Language.LocalizedString(91041);
        GetText((int)Texts.Logout_Text).text = Managers.Language.LocalizedString(91046);
    }
}
