using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public class UI_AccountErrorPopup : UI_Popup
{

    private enum Buttons
    {
        Close_Button,
        Cancle_Button,
        Ok_Button
    }


    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindButtons(typeof(Buttons));

        GetButton((int)Buttons.Close_Button).gameObject.BindEvent(OnEvent_ClickClose, EUIEvent.Click);
        GetButton((int)Buttons.Cancle_Button).gameObject.BindEvent(OnEvent_ClickClose, EUIEvent.Click);
        GetButton((int)Buttons.Ok_Button).gameObject.BindEvent(OnEvent_ClickOk, EUIEvent.Click);


        return true;
    }

    void OnEvent_ClickClose(PointerEventData eventData)
    {
        Managers.UI.ClosePopupUI(this);
    }

    void OnEvent_ClickOk(PointerEventData eventData)
    {
        Managers.Scene.LoadScene(EScene.SignInScene);
    } 
}
