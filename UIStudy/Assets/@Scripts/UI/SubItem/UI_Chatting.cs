using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Chatting : UI_Base
{
    private enum GameObjects
    {
        ChattingRoot,
    }

    private enum Texts
    {
        Nickname_Text,
        Message_Text,
    }
    private enum Buttons
    {
        Send_Button
    }
    private enum InputFields
    {
        Chatting_InputField
    }
    private ChattingStruct _chattingStruct;
    private Transform _chattingRoot;
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindObjects(typeof(GameObjects));
        BindTexts(typeof(Texts));
        BindButtons(typeof(Buttons));
        BindInputFields(typeof(InputFields));
        _chattingRoot = GetObject((int)GameObjects.ChattingRoot).transform;

        return true;
    }
    private void OnClick_SendChatting(PointerEventData eventData)
    {
        _chattingStruct = Managers.Chatting.GetChattingStruct();
        GetText((int)Texts.Nickname_Text).text = _chattingStruct.SenderNickname;
        GetText((int)Texts.Message_Text).text = _chattingStruct.Message;
    }
}
