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
            // 게임 난이도 초기화
            Managers.Game.DifficultySettingsInfo.StageId = 70001;
            Managers.Game.DifficultySettingsInfo.StageLevel = 1; // 이 값을 미션 달성에 사용할 때가 있기 떄문에. 그런데 게임을 새로 시작하면 미션 진행도도 초기화가 되기 때문에 이걸 따로 저장해놔야함.
            Managers.Game.DifficultySettingsInfo.AddSpeed = 0;
            Managers.Game.DifficultySettingsInfo.ChallengeScale = 0;
            // 한 게임당 누적 돌 개수 초기화
            Managers.Game.DifficultySettingsInfo.StoneCount = 0;
            
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