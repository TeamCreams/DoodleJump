using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public class UI_PausePopup : UI_Popup
{
    private enum Texts
    {
        Restart_Text,
        Continue_Text,
        GiveUp_Text
    }

    private enum Buttons
    {
        Restart_Button,
        Continue_Button,
        GiveUp_Button
    }
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindTexts(typeof(Texts));
        BindButtons(typeof(Buttons));

        Managers.Event.AddEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
        //Managers.Event.TriggerEvent(EEventType.SetLanguage);

        GetButton((int)Buttons.Restart_Button).gameObject.BindEvent(OnClick_RestartButton, EUIEvent.Click);
        GetButton((int)Buttons.Continue_Button).gameObject.BindEvent(OnClick_ContinueButton, EUIEvent.Click);
        GetButton((int)Buttons.GiveUp_Button).gameObject.BindEvent(OnClick_GiveUpButton, EUIEvent.Click);

        Time.timeScale = 0;

        return true;
    }
    private void OnClick_RestartButton(PointerEventData eventData)
    {
        Managers.UI.ClosePopupUI(this);
        Managers.Scene.LoadScene(EScene.SuberunkerScene);
    }
    private void OnClick_ContinueButton(PointerEventData eventData)
    {
        Managers.UI.ClosePopupUI(this);
        Time.timeScale = 1;
    }
    private void OnClick_GiveUpButton(PointerEventData eventData)
    {
        Managers.UI.ClosePopupUI(this);
        Managers.Scene.LoadScene(EScene.SuberunkerSceneHomeScene);
    }
    void OnEvent_SetLanguage(Component sender, object param)
    {
        GetText((int)Texts.Restart_Text).text = Managers.Language.LocalizedString(91019);
        GetText((int)Texts.Continue_Text).text = Managers.Language.LocalizedString(91019);
        GetText((int)Texts.GiveUp_Text).text = Managers.Language.LocalizedString(91019);
    }
}
