﻿using UnityEngine;
using Newtonsoft.Json;
using System.Net.Http;
using GameApi.Dtos;
using System;

public class ScoreManager
{

    public void GetScore(Component sender, Action ProcessErrorFun = null, Action onSuccess = null, Action onFailed = null)
    {
        Debug.Log($"{nameof(GetScore)} Call");
        Managers.WebContents.GetUserAccount(new ReqDtoGetUserAccount()
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
                Managers.Game.DifficultySettingsInfo.StageLevel = response.StageLevel;
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
            UI_ErrorButtonPopup.ShowErrorButton(Managers.Error.GetError(Define.EErrorCode.ERR_NetworkSettlementErrorResend), ProcessErrorFun);
            onFailed?.Invoke();
       });
    }
    
    public void SetScore(Component sender, Action onSuccess = null, Action onFailed = null)
    {
        Managers.WebContents.InsertUserAccountScore(new ReqDtoInsertUserAccountScore()
        {
            UserAccountId = Managers.Game.UserInfo.UserAccountId,
            Score = Managers.Game.UserInfo.LatelyScore,
            Time = Managers.Game.GetScore.LatelyPlayTime,
            AccumulatedStone = Managers.Game.DifficultySettingsInfo.StoneCount,
            StageLevel = Managers.Game.DifficultySettingsInfo.StageLevel,
            Gold = Managers.Game.Gold, // 추가할 금액
        },
       (response) =>
       {
            Managers.UI.ShowPopupUI<UI_RetryPopup>();
            onSuccess?.Invoke();
       },
       (errorCode) =>
       {
            UI_ErrorButtonPopup.ShowErrorButton(Managers.Error.GetError(Define.EErrorCode.ERR_NetworkSettlementErrorResend));
       });
    }
}