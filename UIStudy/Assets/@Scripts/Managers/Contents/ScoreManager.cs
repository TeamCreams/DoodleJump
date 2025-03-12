using UnityEngine;
using Newtonsoft.Json;
using System.Net.Http;
using GameApi.Dtos;
using System;

public class ScoreManager
{

    public void GetScore(Component sender, Action ProcessErrorFun = null, Action onSuccess = null, Action onFailed = null)
    {
        Debug.Log($"{nameof(GetScore)} Call");
        Managers.WebContents.ReqGetUserAccount(new ReqDtoGetUserAccount()
        {
            UserName = Managers.Game.UserInfo.UserName,
            Password = Managers.Game.UserInfo.Password
        },
       (response) =>
       {    
            if(response != null)
            {
                Managers.Game.UserInfo.RecordScore = response.HighScore;
                Managers.Game.UserInfo.LatelyScore = response.LatelyScore;
                Managers.Game.UserInfo.Gold = response.Gold;
                Managers.Game.UserInfo.PlayTime = response.PlayTime;
                Managers.Game.UserInfo.AccumulatedStone = response.AccumulatedStone;
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
            ErrorStruct errorStruct = Managers.Error.GetError(Define.EErrorCode.ERR_NetworkSettlementErrorResend);
            popup.SetInfo(errorStruct.Notice, ProcessErrorFun);
            onFailed?.Invoke();
       });
    }
    
    public void SetScore(Component sender, Action onSuccess = null, Action onFailed = null)
    {
        Managers.WebContents.ReqInsertUserAccountScore(new ReqDtoInsertUserAccountScore()
        {
            UserAccountId = Managers.Game.UserInfo.UserAccountId,
            Score = Managers.Game.UserInfo.LatelyScore,
            Time = Managers.Game.UserInfo.PlayTime,
            AccumulatedStone = Managers.Game.DifficultySettingsInfo.StoneCount,
            Gold = Managers.Game.Gold, // 추가할 금액
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
            ErrorStruct errorStruct = Managers.Error.GetError(Define.EErrorCode.ERR_NetworkSettlementErrorResend);
            popup.SetInfo(errorStruct.Notice);
       });
    }
}