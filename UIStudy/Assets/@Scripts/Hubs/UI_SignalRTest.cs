using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public class UI_SignalRTestScene : UI_Scene
{
    private enum InputFields
    {
        Message_InputField
    }

    private enum Buttons
    {
        Enter_Button,
    }

    private enum Texts
    {
        User_Text,
        Message_Text,
    }
    private string _user = "";
    private string _message = "";
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindInputFields(typeof(InputFields));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        GetButton((int)Buttons.Enter_Button).gameObject.BindEvent(OnClick_Enter, EUIEvent.Click);
        GetButton((int)InputFields.Message_InputField).gameObject.BindEvent(OnClick_InputText, EUIEvent.Click);

        return true;
    }

    private void OnClick_Enter(PointerEventData eventData)
    {
        _message = GetInputField((int)InputFields.Message_InputField).text;
        Managers.SignalR.SendMessageAll(Managers.Game.UserInfo.UserAccountId, _message);
        GetInputField((int)InputFields.Message_InputField).text = "";
    }
    private void OnClick_InputText(PointerEventData eventData)
    {
        _message = "";
    }
    private void Display()
    {
        GetText((int)Texts.User_Text).text = _user;
        GetText((int)Texts.Message_Text).text = _message;
        Debug.Log($"[{_user}] {_message}");

    }

}
