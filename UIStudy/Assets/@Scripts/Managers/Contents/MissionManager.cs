using System;
using System.Collections.Generic;
using GameApi.Dtos;
using UnityEngine;
using static Define;
public class MissionManager
{
    public void Init()
    {
        Managers.Event.RemoveEvent(EEventType.OnSettlementComplete, Event_OnSettlementComplete);
        Managers.Event.AddEvent(EEventType.OnSettlementComplete, Event_OnSettlementComplete);
        Managers.Event.RemoveEvent(EEventType.OnFirstAccept, Event_OnFirstAccept);
        Managers.Event.AddEvent(EEventType.OnFirstAccept, Event_OnFirstAccept);
    }

    #region OnEvents
    void Event_OnSettlementComplete(Component sender, object param)
    {
        // Time 계산
        //아에 안들어옴  

         SettleScore(sender);
    }

    void Event_OnFirstAccept(Component sender, object param)
    {
        AcceptMission(sender);
    }

    #endregion

    public void SettleScore(Component sender, Action onSuccess = null, Action onFailed = null)
    {
        Managers.WebContents.ReqGetUserAccount(new ReqDtoGetUserAccount()
        {
            UserName = Managers.Game.UserInfo.UserId
        },
       (response) =>
       {    
            if(response != null)
            {
                Managers.Game.UserInfo.TotalScore = response.TotalScore;
                Managers.Game.UserInfo.UserAccountId = response.UserAccountId;
                Debug.Log("is success");
                onSuccess?.Invoke();
            }
            else
            {                
                Debug.Log("response is null");
                onFailed?.Invoke();
            }
       },
       (errorCode) =>
       {
            UI_ErrorButtonPopup popup = Managers.UI.ShowPopupUI<UI_ErrorButtonPopup>();
            Managers.Event.TriggerEvent(Define.EEventType.ErrorButtonPopup, sender, 
                "The settlement could not be processed due to poor network conditions. Would you like to resend it?");
            popup.AddOnClickAction(onFailed);
       });
    }

    public void AcceptMission(Component sender, Action onSuccess = null, Action onFailed = null)
    {
        Managers.WebContents.ReqInsertUserMission(new ReqDtoInsertUserMission()
        {
            UserAccountId = Managers.Game.UserInfo.UserAccountId,
            MissionId = 11001
        },
       (response) =>
       {    
            if(response != null)
            {
                Debug.Log("AcceptMission is success");
                onSuccess?.Invoke();
            }
            else
            {                
                Debug.Log("AcceptMission response is null");
                onFailed?.Invoke();
            }
       },
       (errorCode) =>
       {
            UI_ErrorButtonPopup popup = Managers.UI.ShowPopupUI<UI_ErrorButtonPopup>();
            Managers.Event.TriggerEvent(Define.EEventType.ErrorButtonPopup, sender, 
                "The settlement could not be processed due to poor network conditions. Would you like to resend it?");
            popup.AddOnClickAction(onFailed);
       });
    }
}
