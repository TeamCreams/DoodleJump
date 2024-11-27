using UnityEngine;
using Newtonsoft.Json;
using System.Net.Http;
using GameApi.Dtos;
using System;

public class ScoreManager
{

    public void GetScore(Component sender, Action ProcessErrorFun = null, Action onSuccess = null, Action onFailed = null)
    {
        Managers.WebContents.ReqGetUserAccount(new ReqDtoGetUserAccount()
        {
            UserName = Managers.Game.UserInfo.UserId
        },
       (response) =>
       {    
            if(response != null)
            {
                Managers.Game.UserInfo.RecordScore = response.HighScore;
                Managers.Game.UserInfo.LatelyScore = response.LatelyScore;
                Managers.Game.UserInfo.Gold = response.Gold;
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
                Debug.Log("is failllll");
            popup.AddOnClickAction(ProcessErrorFun);
            onFailed?.Invoke();
       });
    }
    
    public void SetScore(Component sender, Action onSuccess = null, Action onFailed = null)
    {
        Managers.WebContents.ReqInsertUserAccountScore(new ReqDtoInsertUserAccountScore()
        {
            UserAccountId = Managers.Game.UserInfo.UserAccountId,
            Score = Managers.Game.UserInfo.LatelyScore,
            Gold = Managers.Game.UserInfo.Gold,
        },
       (response) =>
       {
            Managers.UI.ShowPopupUI<UI_RetryPopup>();
            onSuccess?.Invoke();
            /*
            if(response != null)
            {
                Managers.UI.ShowPopupUI<UI_RetryPopup>();
                onSuccess?.Invoke();
            }
            else
            {   
                UI_ErrorButtonPopup popup = Managers.UI.ShowPopupUI<UI_ErrorButtonPopup>();
                Managers.Event.TriggerEvent(Define.EEventType.ErrorButtonPopup, sender, 
                    "The settlement could not be processed due to poor network conditions. Would you like to resend it?");
                popup.AddOnClickAction(ProcessErrorFun);
                onFailed?.Invoke();
            }
            */
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