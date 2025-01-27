using System;
using System.Collections.Generic;
using GameApi.Dtos;
using NUnit.Framework;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.VersionControl;
using UnityEngine;
using static Define;
public class MissionManager
{
    public void Init()
    {
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
        ProcessUserMissionList();
    }

    void Event_OnFirstAccept(Component sender, object param)
    {
        List<ReqDtoInsertUserMission> list = new();
        foreach(var mission in Managers.Data.MissionDataDic)
        {
            ReqDtoInsertUserMission tempMission = new ReqDtoInsertUserMission();
            tempMission.UserAccountId = Managers.Game.UserInfo.UserAccountId;
            tempMission.MissionId = mission.Value.Id;
            list.Add(tempMission);
        }
        AcceptMission(sender, list);
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
           Managers.Game.UserInfo.TotalScore = response.TotalScore;
           Debug.Log($"SettleScore is success, Managers.Game.UserInfo.TotalScore {Managers.Game.UserInfo.TotalScore}");
           onSuccess?.Invoke();
       },
       (errorCode) =>
       {
           onFailed?.Invoke();
           Debug.Log("SettleScore is error");

            UI_ErrorButtonPopup popup = Managers.UI.ShowPopupUI<UI_ErrorButtonPopup>();
            (string title, string notice) = Managers.Error.GetError(EErrorCode.ERR_NetworkSettlementError);
            Managers.Event.TriggerEvent(EEventType.ErrorButtonPopup, sender, notice);
            popup.AddOnClickAction(onFailed);
       });
    }

    public void AcceptMission(Component sender, List<ReqDtoInsertUserMission> missionList, Action onSuccess = null, Action onFailed = null)
    {
        Managers.WebContents.ReqInsertUserMission(new ReqDtoInsertUserMissions()
        {
            UserAccountId = Managers.Game.UserInfo.UserAccountId,
            List = missionList
        },
       (response) =>
       {    
            onSuccess?.Invoke();
       },
       (errorCode) =>
       {
           onFailed?.Invoke();
           if (errorCode == WebApi.Models.Dto.EStatusCode.MissionAlreadyExists)
           {
               // 정상 동작
               return;
           }

           Debug.Log($"AcceptMission is Error {errorCode.ToString()}");

           UI_ErrorButtonPopup popup = Managers.UI.ShowPopupUI<UI_ErrorButtonPopup>();
           (string title, string notice) = Managers.Error.GetError(EErrorCode.ERR_NetworkSettlementErrorResend);
            Managers.Event.TriggerEvent(EEventType.ErrorButtonPopup, sender, notice);
           popup.AddOnClickAction(onFailed);

       });
    }

    public void CompleteMission(Component sender, int missionId, Action onSuccess = null, Action onFailed = null)
    {
        Managers.WebContents.CompleteUserMission(new ReqDtoCompleteUserMission()
        {
            UserAccountId = Managers.Game.UserInfo.UserAccountId,
            MissionId = missionId,
            Gold = Managers.Game.UserInfo.Gold + Managers.Data.MissionDataDic[missionId].Compensation
         },
       (response) =>
       {
           Debug.Log("CompleteUserMission is success");
           Managers.Game.UserInfo.Gold += Managers.Data.MissionDataDic[missionId].Compensation;
           Managers.Event.TriggerEvent(EEventType.UIRefresh);
           onSuccess?.Invoke();
       },
       (errorCode) =>
       {                
            Debug.Log("CompleteUserMission is Error");
           onFailed?.Invoke();
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
           onSuccess?.Invoke();
       },
       (errorCode) =>
       {
             onFailed?.Invoke();
             Debug.Log($"AcceptMission is Error {errorCode}");
           // UI_ErrorButtonPopup popup = Managers.UI.ShowPopupUI<UI_ErrorButtonPopup>();
           // Managers.Event.TriggerEvent(EEventType.ErrorButtonPopup, sender, 
           //     "The settlement could not be processed due to poor network conditions. Would you like to resend it?");
           // popup.AddOnClickAction(onFailed);
       });
    }

    public void ProcessUserMissionList()
    {
        Managers.WebContents.ReqDtoGetUserMissionList(new ReqDtoGetUserMissionList()
        {
            UserAccountId = Managers.Game.UserInfo.UserAccountId
        },
       (response) =>
       {
           if (response != null)
           {
               foreach (var mission in response.List)
               {
                   EMissionType type = Managers.Data.MissionDataDic[mission.MissionId].MissionType;
                   int missionValue = type.GetMissionValueByType();
                   Managers.Event.TriggerEvent(EEventType.OnUpdateMission, null, (mission.MissionId, missionValue));
               }
           }
       },
       (errorCode) =>
       {
            Managers.UI.ShowPopupUI<UI_ToastPopup>();
            (string title, string notice) = Managers.Error.GetError(EErrorCode.ERR_NetworkSettlementError);
            Managers.Event.TriggerEvent(EEventType.ToastPopupNotice, null, notice);
       });
    }


}