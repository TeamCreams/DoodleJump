using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public class UI_Chatting : UI_Base
{
    private enum GameObjects
    {
        ChattingRoot,
    }
    private enum Buttons
    {
        Send_Button
    }
    private enum InputFields
    {
        Chatting_InputField
    }
    private Transform _root;
    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }
        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindInputFields(typeof(InputFields));
        _root = GetObject((int)GameObjects.ChattingRoot).transform;
        Managers.Game.ChattingInfo.Root = _root;
        GetButton((int)Buttons.Send_Button).gameObject.BindEvent(OnClick_SendChatting, EUIEvent.Click);
        Managers.Event.AddEvent(EEventType.ReceiveMessage, Event_ReceiveMessage);
        return true;
    }
    void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.ReceiveMessage, Event_ReceiveMessage);
    }
    private void OnClick_SendChatting(PointerEventData eventData)
    {
        string message = GetInputField((int)InputFields.Chatting_InputField).text;
        if(!string.IsNullOrEmpty(message))
        {
            //Event_ReceiveMessage(null, null); // 얘는 됨
            Managers.SignalR.SendMessageAll(Managers.Game.UserInfo.UserAccountId, message);
            GetInputField((int)InputFields.Chatting_InputField).text = "";
        }
    }

    private void Event_ReceiveMessage(Component component, object param)
    {
        //Debug.Log("Event_ReceiveMessage");
        ChattingStruct chattingStruct = Managers.Chatting.GetChattingStruct();
        Debug.Log($"Event_ReceiveMessage : {chattingStruct.Message}");

        var bubble = Managers.UI.MakeSubItem<UI_ChattingItem>(parent: _root);
        if (bubble == null)
        {
            Debug.Log("Bubble이 생성되지 않음.");
        }
        else
        {
            Debug.Log("Bubble이 성공적으로 생성됨.");
            bubble.SetInfo(chattingStruct);
        }
    }

}
