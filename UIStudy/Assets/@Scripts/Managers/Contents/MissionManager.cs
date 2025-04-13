using System;
using System.Collections.Generic;
using GameApi.Dtos;
using UnityEngine;
using static Define;

// 1. element 간의 호환성이 떨어짐
// 2-1. insertUserMission 뒤에 processUserMisionList를 접근한다
// 2-2. insertUserMission와 processUserMisionList가 하는 일을 하나로 합친다

public class MissionManager
{
    //<MissionId, >
    private Dictionary<int, ResDtoGetUserMissionListElement> _dicts = new Dictionary<int, ResDtoGetUserMissionListElement>();
    public IReadOnlyDictionary<int, ResDtoGetUserMissionListElement> Dicts => _dicts;
    private List<ReqDtoUpdateUserMissionListElement> _changedMissionList = new List<ReqDtoUpdateUserMissionListElement>(); //프로퍼티로 수정
    public IReadOnlyList<ReqDtoUpdateUserMissionListElement> ChangedMissionList => _changedMissionList;

    public void Init()
    {
        Managers.Event.RemoveEvent(EEventType.OnSettlementComplete, Event_OnSettlementComplete);
        Managers.Event.RemoveEvent(EEventType.OnFirstAccept, Event_OnFirstAccept);
        Managers.Event.RemoveEvent(EEventType.OnMissionComplete, Event_OnMissionComplete);
        //Managers.Event.RemoveEvent(EEventType.OnUpdateMission, Event_OnUpdateMission);
        Managers.Event.RemoveEvent(EEventType.OnLogout, Event_ClearMission);

        Managers.Event.AddEvent(EEventType.OnSettlementComplete, Event_OnSettlementComplete);
        Managers.Event.AddEvent(EEventType.OnFirstAccept, Event_OnFirstAccept);
        Managers.Event.AddEvent(EEventType.OnMissionComplete, Event_OnMissionComplete);
        //Managers.Event.AddEvent(EEventType.OnUpdateMission, Event_OnUpdateMission);
        Managers.Event.AddEvent(EEventType.OnLogout, Event_ClearMission);
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
        AcceptMissionList(sender);
    }

    void Event_OnMissionComplete(Component sender, object param)
    {
        CompleteMission(sender, (int)param);
        // 새로운 미션으로 refresh
    }

    // void Event_OnUpdateMission(Component sender, object param)
    // {
    //     //UpdateMissionList(sender); // 원래 주석처리 중이었는데 왜인지 모르곘네
    // }

    void Event_ClearMission(Component sender, object param)
    {
        ClearMission();
    }

    #endregion
    public void SettleScore(Component sender, Action onSuccess = null, Action onFailed = null)
    {
        Debug.Log($"{nameof(SettleScore)} Call");
        Managers.Game.UserInfo.RecordScore = Math.Max(Managers.Game.UserInfo.RecordScore, Managers.Game.UserInfo.LatelyScore);

        // 1. _dicts 업데이트
        foreach (var missionKeyValue in _dicts)
        {
            int missionId = missionKeyValue.Key;
            ResDtoGetUserMissionListElement mission = missionKeyValue.Value;
            EMissionType type = Managers.Data.MissionDataDic[missionId].MissionType;

            int beforeParam1 = mission.Param1;
            mission.Param1 = type.GetMissionValueByType();
            Debug.Log($"mission.Param1 {mission.Param1}");
            if(Managers.Mission.Dicts[missionId].MissionStatus != (int)EMissionStatus.Rewarded)
            {
                Debug.Log("Changed Call");
                float value = beforeParam1 / (float)Managers.Data.MissionDataDic[missionId].Param1;
                if(value < 1.0f)
                {
                    Managers.Mission.Dicts[missionId].MissionStatus = (int)EMissionStatus.Progress;
                }
                else
                {
                    Managers.Mission.Dicts[missionId].MissionStatus = (int)EMissionStatus.Complete;
                }
            }
            mission.MissionStatus = Managers.Mission.Dicts[missionId].MissionStatus;

            if(beforeParam1 != mission.Param1)
            {
                // 변경된것 저장.
                ReqDtoUpdateUserMissionListElement element = new ReqDtoUpdateUserMissionListElement(); 
                element.MissionId = mission.MissionId;
                element.Param1 = mission.Param1;
                element.MissionStatus = mission.MissionStatus;
                _changedMissionList.Add(element);
            }
        }

        // 2. 변경된 _dicts를 서버에 변경 요청을 보낸다.
        UpdateMissionList(sender);

        // 3. onsuccess에서  UI 업데이트 요청
        
    }

    public void AcceptMissionList(Component sender, Action onSuccess = null, Action onFailed = null)
    {
        // 새로운 미션 추가
        // 새로운 업데이트가 되어 있을 수도 있으니까
        // 리스트를 보내면 서버에서 없는 것만 추가하도록 짜여져 있음.
        Debug.Log($"AcceptMissionList");

        List<ReqDtoInsertUserMissionListElement> list = new List<ReqDtoInsertUserMissionListElement>();
        foreach (var mission in Managers.Data.MissionDataDic)
        {
            ReqDtoInsertUserMissionListElement element = new ReqDtoInsertUserMissionListElement();
            element.UserAccountId = Managers.Game.UserInfo.UserAccountId;
            element.MissionId = mission.Value.Id;
            list.Add(element);
        }
    
        Managers.WebContents.InsertUserMission(new ReqDtoInsertUserMissionList()
        {
            UserAccountId = Managers.Game.UserInfo.UserAccountId,
            List = list
        },
       (response) =>
       { 
            onSuccess?.Invoke();
            foreach (var mission in response.List)
            {
                if (!_dicts.TryGetValue(mission.MissionId, out var existingElement))
                {
                    //Debug.Log($"{mission.MissionId} Param1 : {mission.Param1}");
                    var newElement = new ResDtoGetUserMissionListElement
                    {
                        MissionId = mission.MissionId,
                        MissionStatus = mission.MissionStatus,
                        Param1 = mission.Param1
                    };
                    _dicts[mission.MissionId] = newElement;
                }
                else
                {
                    //Debug.Log($"existingElement.MissionStatus : {existingElement.MissionStatus}, mission.Param1 : {mission.Param1}");
                    _dicts[mission.MissionId].MissionStatus = mission.MissionStatus;
                    _dicts[mission.MissionId].Param1 = mission.Param1;
                }
            }
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

            UI_ErrorButtonPopup.ShowErrorButton(Managers.Error.GetError(EErrorCode.ERR_NetworkSettlementErrorResend));
       });
    }

    public void CompleteMission(Component sender, int missionId, Action onSuccess = null, Action onFailed = null)
    {
    //     EMissionType type = Managers.Data.MissionDataDic[missionId].MissionType;
    //     int missionValue = type.GetMissionValueByType();

        Managers.WebContents.CompleteUserMission(new ReqDtoCompleteUserMission()
        {
            UserAccountId = Managers.Game.UserInfo.UserAccountId,
            MissionId = missionId,
            Param1 = _dicts[missionId].Param1,
            Gold = Managers.Data.MissionDataDic[missionId].Compensation
         },
       (response) =>
       {
            onSuccess?.Invoke();
            Debug.Log("CompleteUserMission is success");
            Managers.Game.GoldTochange = response.Gold;
            foreach (var mission in response.List)
            {
                _dicts[mission.MissionId].MissionStatus = mission.MissionStatus;
                _dicts[mission.MissionId].Param1 = mission.Param1;
            }
            Managers.Event.TriggerEvent(EEventType.AddGold);
            Managers.Event.TriggerEvent(EEventType.UIRefresh);
            Managers.Event.TriggerEvent(EEventType.Mission);
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

    public void UpdateMissionList(Component sender, Action onSuccess = null, Action onFailed = null)
    {
        Debug.Log($"{nameof(UpdateMissionList)} Call");
        if(_changedMissionList.Count < 1)
        {
            return;
        }
        Managers.WebContents.UpdateUserMissionList(new ReqDtoUpdateUserMissionList()
        {
            UserAccountId = Managers.Game.UserInfo.UserAccountId,
            List = _changedMissionList
        },
       (response) =>
       {
            onSuccess?.Invoke();
            foreach (var mission in response.List)
            {
                Debug.Log($"_dicts[{mission.MissionId}].Param1 : {_dicts[mission.MissionId].Param1}, mission.Param1 : {mission.Param1}");
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
        Managers.WebContents.GetUserMissionList(new ReqDtoGetUserMissionList()
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
            UI_ToastPopup.ShowError(Managers.Error.GetError(EErrorCode.ERR_NetworkSettlementError));
       });
    }
    public void ClearMission()
    {
        _dicts = new Dictionary<int, ResDtoGetUserMissionListElement>();
    }
}