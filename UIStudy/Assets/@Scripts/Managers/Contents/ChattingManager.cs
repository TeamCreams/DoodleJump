using System;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ChattingManager
{
    private ChattingStruct _chattingStruct;
    
    public void Event_SendMessage(ChattingStruct chatting)
    {
        _chattingStruct.IsPrivateMessage = chatting.IsPrivateMessage;
        _chattingStruct.Message = chatting.Message;
    }
    public void Event_ReceiveMessage()
    {
        Debug.Log($"Event_ReceiveMessage");
        var bubble = Managers.Resource.Instantiate("UI_ChattingItem"); //생성조차 안 됨
        bubble.GetOrAddComponent<UI_ChattingItem>().SetInfo(_chattingStruct);
    }
    public ChattingStruct GetChattingStruct() // raedOnly가 안 됨
    {
        return _chattingStruct;
    }
}