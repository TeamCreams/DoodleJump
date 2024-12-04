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
        Language_Text
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
        //OnEvent_SetLanguage(null, null);

        GetButton((int)Buttons.Close_Button).gameObject.BindEvent(OnClick_CloseButton, EUIEvent.Click);
        GetToggle((int)Toggles.Language_En).gameObject.BindEvent(OnClick_SetLanguage, EUIEvent.Click);
        GetToggle((int)Toggles.Language_Kr).gameObject.BindEvent(OnClick_SetLanguage, EUIEvent.Click);
        GetSlider((int)Sliders.Music_Slider).gameObject.BindEvent(OnDrag_MusicSlider, EUIEvent.Drag);
        GetSlider((int)Sliders.SoundFx_Slider).gameObject.BindEvent(OnDrag_SoundFxSlider, EUIEvent.Drag);

        return true;
    }
    private void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
    }

    private void OnClick_CloseButton(PointerEventData eventData)
    {
        Managers.UI.ClosePopupUI(this);
    }

    private void OnClick_SetLanguage(PointerEventData eventData)
    {
        if(GetToggle((int)Toggles.Language_En).isOn == true)
        {
            Managers.Language.ELanguageInfo = ELanguage.En;
        }
        else
        {
            Managers.Language.ELanguageInfo = ELanguage.Kr;
        }
        Managers.Event.TriggerEvent(EEventType.SetLanguage);
        Debug.Log($"language : {Managers.Language.ELanguageInfo}");
    }

    private void OnDrag_MusicSlider(PointerEventData eventData)
    {
        float f = 0;
        f = GetSlider((int)Sliders.Music_Slider).value;
        Debug.Log($"Music : {f}");
    }

    private void OnDrag_SoundFxSlider(PointerEventData eventData)
    {
        float f = 0;
        f = GetSlider((int)Sliders.SoundFx_Slider).value;
        Debug.Log($"SoundFx : {f}");
    }
    
    void OnEvent_SetLanguage(Component sender, object param)
    {
        GetText((int)Texts.Setting_Text).text = Managers.Language.LocalizedString(91019);
        GetText((int)Texts.Music_Text).text = Managers.Language.LocalizedString(91019);
        GetText((int)Texts.SoundFx_Text).text = Managers.Language.LocalizedString(91019);
        GetText((int)Texts.Vibration_Text).text = Managers.Language.LocalizedString(91019);
        GetText((int)Texts.Language_Text).text = Managers.Language.LocalizedString(91019);
    }
}
