using System;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ChattingManager
{
    private ChattingStruct _chattingStruct;
    public void Init()
	{
        Managers.Event.RemoveEvent(EEventType.ReceiveMessage, Event_DisplaySendMessageAll);
		Managers.Event.AddEvent(EEventType.ReceiveMessage, Event_DisplaySendMessageAll);
	}

    public void Event_DisplaySendMessageAll(Component sender, object param)
    {
        if(param is ChattingStruct chatting)
        {
            _chattingStruct.SenderUserId = chatting.SenderUserId;
            // 아이디로 유저닉네임 가져오기
            _chattingStruct.SenderNickname = Managers.Game.UserInfo.UserNickname;
            _chattingStruct.Message = chatting.Message;
        }
    }
    public ChattingStruct GetChattingStruct() // raedOnly가 안 됨
    {
        return _chattingStruct;
    }

}