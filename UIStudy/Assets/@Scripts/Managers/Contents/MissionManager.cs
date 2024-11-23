using System;
using System.Collections.Generic;
using GameApi.Dtos;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.VersionControl;
using UnityEngine;
using static Define;
public class MissionManager
{
    public void Init()
    {
        Debug.Log("MissionManager Init");

        Managers.Event.RemoveEvent(EEventType.OnSettlementComplete, Event_OnSettlementComplete);
        Managers.Event.RemoveEvent(EEventType.OnFirstAccept, Event_OnFirstAccept);
        Managers.Event.RemoveEvent(EEventType.OnMissionComplete, Event_OnMissionComplete);
        Managers.Event.RemoveEvent(EEventType.OnUpdateMission, Event_OnUpdateMission);

        Managers.Event.AddEvent(EEventType.OnSettlementComplete, Event_OnSettlementComplete);
        Managers.Event.AddEvent(EEventType.OnFirstAccept, Event_OnFirstAccept);
        Managers.Event.AddEvent(EEventType.OnMissionComplete, Event_OnMissionComplete);
        Managers.Event.AddEvent(EEventType.OnUpdateMission, Event_OnUpdateMission);
    }

    #region OnEvents
    void Event_OnSettlementComplete(Component sender, object param)
    {
        // Time 계산
        Debug.Log("Event_OnSettlementComplete");
         SettleScore(sender);
    }

    void Event_OnFirstAccept(Component sender, object param)
    {
        foreach(var mission in Managers.Data.MissionDataDic)
        {
            AcceptMission(sender, mission.Value.Id);
        }
    }

    void Event_OnMissionComplete(Component sender, object param)
    {
        Debug.Log("Event_OnMissionComplete");
        int id = (int)param;
        CompleteMission(sender, id);
    }

    void Event_OnUpdateMission(Component sender, object param)
    {
        Debug.Log("Event_OnUpdateMission");
        int missionId = 0;
        int param1 = 0;
        if(param is ValueTuple<int, int> data)
        {
            missionId = data.Item1;
            param1 = data.Item2;
        }
        UpdateMission(sender, missionId, param1);
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
                Debug.Log($"SettleScore is success, Managers.Game.UserInfo.TotalScore {Managers.Game.UserInfo.TotalScore}");
                onSuccess?.Invoke();
            }
            else
            {                
                Debug.Log("SettleScore response is null");
                onFailed?.Invoke();
            }
       },
       (errorCode) =>
       {
            Debug.Log("SettleScore is error");

            UI_ErrorButtonPopup popup = Managers.UI.ShowPopupUI<UI_ErrorButtonPopup>();
            Managers.Event.TriggerEvent(EEventType.ErrorButtonPopup, sender, 
                "The settlement could not be processed due to poor network conditions. Would you like to resend it?");
            popup.AddOnClickAction(onFailed);
       });
    }

    public void AcceptMission(Component sender, int missionId, Action onSuccess = null, Action onFailed = null)
    {
        Managers.WebContents.ReqInsertUserMission(new ReqDtoInsertUserMission()
        {
            UserAccountId = Managers.Game.UserInfo.UserAccountId,
            MissionId = missionId
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
                //Debug.Log($"Message : {response.Message}, StatusCode : {response.statusCode}");
                onFailed?.Invoke();
            }
       },
       (errorCode) =>
       {                
            Debug.Log($"AcceptMission is Error {errorCode}");
            
            // UI_ErrorButtonPopup popup = Managers.UI.ShowPopupUI<UI_ErrorButtonPopup>();
            // Managers.Event.TriggerEvent(EEventType.ErrorButtonPopup, sender, 
            //     "The settlement could not be processed due to poor network conditions. Would you like to resend it?");
            // popup.AddOnClickAction(onFailed);
       });
    }

    public void CompleteMission(Component sender, int missionId, Action onSuccess = null, Action onFailed = null)
    {
        Managers.WebContents.CompleteUserMission(new ReqDtoCompleteUserMission()
        {
            UserAccountId = Managers.Game.UserInfo.UserAccountId,
            MissionId = missionId
        },
       (response) =>
       {    
            if(response != null)
            {
                Debug.Log("CompleteUserMission is success");
                onSuccess?.Invoke();
            }
            else
            {                
                Debug.Log("CompleteUserMission response is null");
                onFailed?.Invoke();
            }
       },
       (errorCode) =>
       {                
            Debug.Log("CompleteUserMission is Error");
            // UI_ErrorButtonPopup popup = Managers.UI.ShowPopupUI<UI_ErrorButtonPopup>();
            // Managers.Event.TriggerEvent(EEventType.ErrorButtonPopup, sender, 
            //     "The settlement could not be processed due to poor network conditions. Would you like to resend it?");
            // popup.AddOnClickAction(onFailed);
       });
    }

    public void UpdateMission(Component sender, int missionId, int param1, Action onSuccess = null, Action onFailed = null)
    {
        Managers.WebContents.UpdateUserMission(new ReqDtoUpdateUserMission()
        {
            UserAccountId = Managers.Game.UserInfo.UserAccountId,
            MissionId = missionId,
            Param1 = param1
        },
       (response) =>
       {    
            if(response != null)
            {
                Debug.Log("UpdateUserMission is success");
                onSuccess?.Invoke();
            }
            else
            {                
                Debug.Log("UpdateUserMission response is null");
                onFailed?.Invoke();
            }
       },
       (errorCode) =>
       {                
            Debug.Log("UpdateUserMission is Error");
            // UI_ErrorButtonPopup popup = Managers.UI.ShowPopupUI<UI_ErrorButtonPopup>();
            // Managers.Event.TriggerEvent(EEventType.ErrorButtonPopup, sender, 
            //     "The settlement could not be processed due to poor network conditions. Would you like to resend it?");
            // popup.AddOnClickAction(onFailed);
       });
    }
}
