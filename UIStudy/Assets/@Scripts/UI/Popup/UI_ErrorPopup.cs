using UnityEngine;
using static Define;

public class UI_ErrorPopup : UI_Popup
{

    private enum Texts
    {
        ErrorTitle_Text,
        Notice_Text
    }

    private enum Buttons
    {
        Close_Button,
    }


    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindTexts(typeof(Texts));
        BindButtons(typeof(Buttons));

        Managers.Event.AddEvent(EEventType.ErrorPopupTitle, OnEvent_ErrorTitle);
        Managers.Event.AddEvent(EEventType.ErrorPopupTitle, OnEvent_Notice);

        GetButton((int)Buttons.Close_Button).gameObject.BindEvent((evt) =>
        {
            Managers.UI.ClosePopupUI(this);
        }, EUIEvent.Click);

        return true;
    }

    void OnEvent_ErrorTitle(Component sender, object param)
    {
        string str = param as string;
        GetText((int)Texts.ErrorTitle_Text).text = str;
    }
    void OnEvent_Notice(Component sender, object param)
    {
        string str = param as string;
        GetText((int)Texts.Notice_Text).text = str;
    }
}
