using System;
using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public class UI_ErrorButtonPopup : UI_Popup
{

    private enum Buttons
    {
        Close_Button,
        Cancle_Button,
        Ok_Button
    }

    private enum Texts
    {
        Notice_Text
    }

    EScene _scene = EScene.SignInScene;

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        GetButton((int)Buttons.Close_Button).gameObject.BindEvent(OnEvent_ClickClose, EUIEvent.Click);
        GetButton((int)Buttons.Cancle_Button).gameObject.BindEvent(OnEvent_ClickClose, EUIEvent.Click);
        //GetButton((int)Buttons.Ok_Button).gameObject.BindEvent(OnEvent_ClickOk, EUIEvent.Click);
        
        Managers.Event.AddEvent(EEventType.ErrorButtonPopup, OnClick_ErrorButtonPopup);

        return true;
    }

    private void OnEvent_ClickClose(PointerEventData eventData)
    {
        Managers.UI.ClosePopupUI(this);
    }

    private void OnEvent_ClickOk(PointerEventData eventData)
    {
        Managers.Scene.LoadScene(_scene);
    } 

    private void OnClick_ErrorButtonPopup(Component sender, object param)
    {
        string str = param as string;
        GetText((int)Texts.Notice_Text).text = str;
    }
    
    public void AddOnClickAction(Action action)
    {
        if(action != null)
        {
            GetButton((int)Buttons.Ok_Button).gameObject.BindEvent((evt) => action?.Invoke(), EUIEvent.Click);
            return;
        }        
        GetButton((int)Buttons.Ok_Button).gameObject.BindEvent((evt) => Managers.UI.ClosePopupUI(this), EUIEvent.Click);
    }
}
