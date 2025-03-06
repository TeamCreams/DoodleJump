using System;
using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public class UI_ErrorButtonPopup : UI_Popup
{

    private enum Buttons
    {
        Cancle_Button,
        Ok_Button
    }

    private enum Texts
    {
        Cancle_Text,
        Ok_Text,
        Notice_Text
    }

    private string _notice;
    private EScene _scene;

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        //Bind
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        //Get
        GetButton((int)Buttons.Cancle_Button).gameObject.BindEvent(OnEvent_ClickClose, EUIEvent.Click);
        GetButton((int)Buttons.Ok_Button).gameObject.BindEvent(OnEvent_ClickOk, EUIEvent.Click);
        
        //add Event
        Managers.Event.AddEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
        return true;
    }
    private void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.SetLanguage, OnEvent_SetLanguage);
    }
    public void SetInfo(string notice, Action action = null, EScene scene = EScene.SignInScene)
    {
        _notice = notice;
        _scene = scene;
        GetText((int)Texts.Notice_Text).text = _notice;
        if(action != null)
        {
            GetButton((int)Buttons.Ok_Button).gameObject.BindEvent((evt) => action?.Invoke(), EUIEvent.Click);
            //return;
        }        
        //GetButton((int)Buttons.Ok_Button).gameObject.BindEvent(OnEvent_ClickClose, EUIEvent.Click);
    }

    private void OnEvent_ClickClose(PointerEventData eventData)
    {
        Managers.UI.ClosePopupUI(this);
    }

    private void OnEvent_ClickOk(PointerEventData eventData)
    {
        Managers.Scene.LoadScene(_scene);
    }

    void OnEvent_SetLanguage(Component sender, object param)
    {
        GetText((int)Texts.Cancle_Text).text = Managers.Language.LocalizedString(91053);
        GetText((int)Texts.Ok_Text).text = Managers.Language.LocalizedString(91054);
    }
}
