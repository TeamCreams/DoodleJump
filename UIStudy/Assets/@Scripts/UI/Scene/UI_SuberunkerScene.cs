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
        Managers.Event.RemoveEvent(EEventType.OnSettlementComplete, Event_OnSettlementComplete);
    }

    #region Events
    void Event_OnPlayerDead(Component sender, object param)
    {
        int tryCount = (int)param;
        if (tryCount == 2)
        {
            Managers.Event.TriggerEvent(EEventType.StopLoading);
            Managers.UI.ShowPopupUI<UI_ToastPopup>();
            (string title, string notice) = Managers.Error.GetError(EErrorCode.ERR_NetworkSaveError);
            Managers.Event.TriggerEvent(EEventType.ToastPopupNotice, this, notice);
            Invoke("ExitGame", 2.5f);
            return;
        }

        
        Managers.Resource.Instantiate("UI_Loading", this.transform);
        Managers.Event.TriggerEvent(EEventType.StartLoading);
        Managers.Score.SetScore(
            this, 
            onSuccess: () => {
                    Managers.Event.TriggerEvent(EEventType.OnSettlementComplete); 
                    Managers.Event.TriggerEvent(EEventType.OnUpdateMission);
                },
            onFailed: () => Event_OnPlayerDead(this, tryCount++));
        Managers.Event.TriggerEvent(EEventType.StopLoading);
    }
    
    void Event_OnSettlementComplete(Component sender, object param)
    {
        
    }

    #endregion

    
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
