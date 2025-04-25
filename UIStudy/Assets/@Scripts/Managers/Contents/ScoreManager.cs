using UnityEngine;
using Newtonsoft.Json;
using System.Net.Http;
using GameApi.Dtos;
using System;

public class ScoreManager
{

    public void GetScore(Component sender, Action onSuccess = null, Action onFailed = null)
    {
        string usernameData = SecurePlayerPrefs.GetString(Define.HardCoding.UserName, Define.HardCoding.UserName);
        string passwordData = SecurePlayerPrefs.GetString(Define.HardCoding.Password, Define.HardCoding.Password); 
        string googleAccountData = SecurePlayerPrefs.GetString(Define.HardCoding.GoogleAccount, Define.HardCoding.GoogleAccount); 

        Debug.Log("1--1 usernameData : " + usernameData);

        Debug.Log("1--1 passwordData : " + passwordData);

        Debug.Log("1--1 googleAccountData : " + googleAccountData);


        // 로그인 방식 결정
        if (!string.IsNullOrEmpty(googleAccountData) && 
            googleAccountData != Define.HardCoding.GoogleAccount && 
            googleAccountData != "0")
        {
            Debug.Log("GetUserAccountByGoogle");
            // 구글 계정 정보가 있으면 구글 계정으로 로그인
            Managers.WebContents.GetUserAccountByGoogle(new ReqDtoGetUserAccountByGoogle()
            {
                GoogleAccount = Managers.Game.UserInfo.GoogleAccount
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
                UI_ErrorButtonPopup.ShowErrorButton(Managers.Error.GetError(Define.EErrorCode.ERR_NetworkSettlementErrorResend), null, Define.EScene.StartLoadingScene);
                onFailed?.Invoke();
            });
        }
        else if (!string.IsNullOrEmpty(usernameData) && !string.IsNullOrEmpty(passwordData) && 
                usernameData != Define.HardCoding.UserName && passwordData != Define.HardCoding.Password)
        {
            Debug.Log("GetUserAccount");

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
                UI_ErrorButtonPopup.ShowErrorButton(Managers.Error.GetError(Define.EErrorCode.ERR_NetworkSettlementErrorResend), null, Define.EScene.StartLoadingScene);
                onFailed?.Invoke();
            });
        }
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
            UI_ErrorButtonPopup.ShowErrorButton(
                Managers.Error.GetError(Define.EErrorCode.ERR_NetworkSettlementErrorResend));
            onFailed?.Invoke();
        }); 
    }
}