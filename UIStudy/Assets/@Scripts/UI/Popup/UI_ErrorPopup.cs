using System;
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

        Managers.Event.AddEvent(EEventType.ErrorPopup, OnEvent_ErrorPopup);

        GetButton((int)Buttons.Close_Button).gameObject.BindEvent((evt) =>
        {
            Managers.UI.ClosePopupUI(this);
        }, EUIEvent.Click);

        return true;
    }

    void OnEvent_ErrorPopup(Component sender, object param)
    {
        if(param is ValueTuple<string, string> data)
        {
            GetText((int)Texts.ErrorTitle_Text).text = data.Item1;
            GetText((int)Texts.Notice_Text).text = data.Item2;
        }
    }
}
