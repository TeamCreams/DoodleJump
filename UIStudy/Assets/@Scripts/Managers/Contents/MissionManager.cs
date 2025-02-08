using System;
using System.Collections.Generic;
using GameApi.Dtos;
using NUnit.Framework;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.VersionControl;
using UnityEngine;
using static Define;
public class MissionManager
{
    //<MissionId, >
    private Dictionary<int, ResDtoGetUserMissionListElement> _dicts = new Dictionary<int, ResDtoGetUserMissionListElement>();
    public IReadOnlyDictionary<int, ResDtoGetUserMissionListElement> Dicts => _dicts;
    public   List<ReqDtoGetOrUpdateUserMissionElement> _changedMissionList = new List<ReqDtoGetOrUpdateUserMissionElement>();


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
    }

    void Event_OnFirstAccept(Component sender, object param)
    {
        //ProcessUserMissionList();
    

        AcceptMission(sender);
    }

    void Event_OnMissionComplete(Component sender, object param)
    {
    }

    void Event_OnUpdateMission(Component sender, object param)
    {
    }

    #endregion



    public void SettleScore(Component sender, Action onSuccess = null, Action onFailed = null)
    {
        // 1. _dicts 업데이트
        ReqDtoGetOrUpdateUserMissionElement a = new ReqDtoGetOrUpdateUserMissionElement(); 
        foreach(var missionKeyValue in _dicts)
        {
            int missionId = missionKeyValue.Key;
            ResDtoGetUserMissionListElement mission = missionKeyValue.Value;
            EMissionType type = Managers.Data.MissionDataDic[missionId].MissionType;

            int beforeParam1 = mission.Param1;
            mission.Param1 = type.GetMissionValueByType();

            if(beforeParam1 != mission.Param1)
            {
                // 변경된것 저장.
                a.MissionId = mission.MissionId;
                a.Param1 = mission.Param1;
                _changedMissionList.Add(a);
            }
        }

        // 2. 변경된 _dicts를 서버에 변경 요청을 보낸다.
        GetOrUpdateMission(sender);

        // 3. onsuccess에서  UI 업데이트 요청

    }

    public void AcceptMission(Component sender, Action onSuccess = null, Action onFailed = null)
    {
        // 새로운 미션 추가
        // 새로운 업데이트가 되어 있을 수도 있으니까
        // 리스트를 보내면 서버에서 없는 것만 추가하도록 짜여져 있음.
        List<ReqDtoInsertUserMissionElement> list = new List<ReqDtoInsertUserMissionElement>();
        foreach (var mission in Managers.Data.MissionDataDic)
        {
            ReqDtoInsertUserMissionElement tempMission = new ReqDtoInsertUserMissionElement();
            tempMission.UserAccountId = Managers.Game.UserInfo.UserAccountId;
            tempMission.MissionId = mission.Value.Id;
            list.Add(tempMission);
        }
    
        Managers.WebContents.ReqInsertUserMission(new ReqDtoInsertUserMissionList()
        {
            UserAccountId = Managers.Game.UserInfo.UserAccountId,
            List = list
        },
       (response) =>
       { 
            onSuccess?.Invoke();
            // 추가 되면 로딩창 끝내도록.
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

    public void GetOrUpdateMission(Component sender, Action onSuccess = null, Action onFailed = null)
    {
        Managers.WebContents.UpdateUserMission(new ReqDtoGetOrUpdateUserMissionList()
        {
            UserAccountId = Managers.Game.UserInfo.UserAccountId,
            List = _changedMissionList
        },
       (response) =>
       {
            onSuccess?.Invoke();
            foreach (var mission in response.List)
            {
                _dicts[mission.MissionId].MissionStatus = mission.MissionStatus;
                _dicts[mission.MissionId].Param1 = mission.Param1;
            }
            //초기화
            _changedMissionList.Clear();
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
                   //EMissionType type = Managers.Data.MissionDataDic[mission.MissionId].MissionType;
                   //int missionValue = type.GetMissionValueByType();
                   _dicts[mission.MissionId] = mission;
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