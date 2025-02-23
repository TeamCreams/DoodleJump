using System;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ChattingManager
{
    public void Init()
	{
        Managers.Event.RemoveEvent(EEventType.ReceiveMessage, Event_DisplayMessage);
		Managers.Event.AddEvent(EEventType.ReceiveMessage, Event_DisplayMessage);
	}

    public void Event_DisplayMessage(Component sender, object param)
    {
        if(param is ChattingStruct chatting)
        {
            //여기 뭔갈.
        }
    }

}