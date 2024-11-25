using System;
using System.Collections;
using GameApi.Dtos;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_SuberunkerScene : UI_Scene
{

    enum GameObjects
    {
    
    }

    enum Texts
    { 
    
    }

    enum Buttons
    {
    
    }

    public override bool Init()
    {
        if (base.Init() == false)
        {
            return false;
        }

        //StartLoadAssets();

        //Managers.Game.OnChangedLife -= RefreshUI;
        //Managers.Game.OnChangedLife += RefreshUI;

        Managers.Event.RemoveEvent(EEventType.OnPlayerDead, Event_OnPlayerDead);
        Managers.Event.RemoveEvent(EEventType.OnSettlementComplete, Event_OnSettlementComplete);
        Managers.Event.AddEvent(EEventType.OnPlayerDead, Event_OnPlayerDead);
        Managers.Event.AddEvent(EEventType.OnSettlementComplete, Event_OnSettlementComplete);

        return true;
    }

    private void OnDestroy()
    {
        Managers.Event.RemoveEvent(EEventType.OnPlayerDead, Event_OnPlayerDead);
    }

    #region Events
    void Event_OnPlayerDead(Component sender, object param)
    {
        int tryCount = (int)param;
        if (tryCount == 2)
        {
            Managers.Event.TriggerEvent(EEventType.StopLoading);
            Managers.UI.ShowPopupUI<UI_ToastPopup>();
            Managers.Event.TriggerEvent(EEventType.ToastPopupNotice, this, "Failed to save...");
            Invoke("ExitGame", 2.5f);
            return;
        }

        
        Managers.Resource.Instantiate("UI_Loading", this.transform);
        Managers.Event.TriggerEvent(EEventType.StartLoading);
        Managers.Score.SetScore(
            this, 
            onSuccess: () => Managers.Event.TriggerEvent(EEventType.OnSettlementComplete),
            onFailed: () => Event_OnPlayerDead(this, tryCount++));
        Managers.Event.TriggerEvent(EEventType.StopLoading);
    }

    void Event_OnSettlementComplete(Component sender, object param)
    {
        /*
        Managers.WebContents.ReqDtoGetUserMissionList(new ReqDtoGetUserMissionList()
        {
            UserAccountId = Managers.Game.UserInfo.UserAccountId
        },
       (response) =>
       {    
            if(response != null)
            {
                foreach(var mission in response.List)
                {
                    EMissionType type = Managers.Data.MissionDataDic[mission.MissionId].MissionType;
                    int missionValue = GetMissionValueByType(type);
                    Managers.Event.TriggerEvent(EEventType.OnUpdateMission, this, (0, missionValue));
                }
                Debug.Log("is success");
            }
       },
       (errorCode) =>
       {
            Managers.UI.ShowPopupUI<UI_ToastPopup>();
            Managers.Event.TriggerEvent(EEventType.ToastPopupNotice, this, "The settlement could not be processed due to poor network conditions.");
       });
        */

    }

    #endregion

    private int GetMissionValueByType(EMissionType type)
    {
        //미션에 맞게 value 값 가져오기
        
        switch(type)
        {
            case EMissionType.Time:
                return Managers.Game.UserInfo.TotalScore;
            case EMissionType.SurviveToLevel:
                return 1;
            case EMissionType.AvoidRocksCount:
                return 1;
            case EMissionType.AchieveScoreInGame:
                return Managers.Game.UserInfo.LatelyScore;
            case EMissionType.Style:
            return 1;
        }
        return 1;
    }
    #region Interface
    private void ExitGame()
    {
        Managers.Scene.LoadScene(EScene.SuberunkerSceneHomeScene);
    }

    void StartLoadAssets()
    {
        Managers.Resource.LoadAllAsync<UnityEngine.Object>("PreLoad", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count}/{totalCount}");

            if (count == totalCount)
            {
                Debug.Log("Load Complete");
                Managers.Data.Init();
            }
        });
    }
    #endregion
}
