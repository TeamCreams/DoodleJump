using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
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

        GetButton((int)Buttons.Close_Button).gameObject.BindEvent(OnClick_ClosePopup, EUIEvent.Click);

        return true;
    }
    public void SetInfo(NoticeInfo data)
    {
        GetText((int)Texts.ErrorTitle_Text).text = data.Title;
        GetText((int)Texts.Notice_Text).text = data.Notice;
    }
    private void OnClick_ClosePopup(PointerEventData eventData)
    {
        Managers.UI.ClosePopupUI(this);
    }
    public static void ShowError(NoticeInfo data)
    {
        UI_ErrorPopup errorPopup = Managers.UI.ShowPopupUI<UI_ErrorPopup>();
        errorPopup.SetInfo(data);
    }
}
