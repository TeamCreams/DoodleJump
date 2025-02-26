using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public class UI_UserInfoPopup : UI_Popup
{
    private enum Texts
    {
        Nickname_Text,
    }

    private enum Buttons
    {
        Close_Button,
        SendWhisper_Button
    }

    private enum InputFields
    {
        Whisper_InputField
    }
    private int _userAccountId; 

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindTexts(typeof(Texts));
        BindButtons(typeof(Buttons));
        BindInputFields(typeof(InputFields));

        GetButton((int)Buttons.Close_Button).gameObject.BindEvent((evt) =>
        {
            Managers.UI.ClosePopupUI(this);
        }, EUIEvent.Click);

        GetButton((int)Buttons.SendWhisper_Button).gameObject.BindEvent(OnClick_SendChatting, EUIEvent.Click);

        return true;
    }

    public void SetInfo(int id, string nickname)
    {
        _userAccountId = id;
        GetText((int)Texts.Nickname_Text).text = nickname;
    }

    private void OnClick_SendChatting(PointerEventData eventData)
    {
        string message = GetInputField((int)InputFields.Whisper_InputField).text;
        if(!string.IsNullOrEmpty(message))
        {
            Managers.SignalR.SendMessageOneToOne(Managers.Game.UserInfo.UserAccountId, _userAccountId, message);
            GetInputField((int)InputFields.Whisper_InputField).text = "";
        }
    }
}
