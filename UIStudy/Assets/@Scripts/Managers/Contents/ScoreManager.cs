using UnityEngine;
using Newtonsoft.Json;
using System.Net.Http;
using GameApi.Dtos;
using System;

public class ScoreManager
{
    ScoreManagerSlave _slave = null;
    public void Init()
    {
        GameObject newObj = new GameObject("@ScoreManagerSlave");
        _slave = newObj.GetOrAddComponent<ScoreManagerSlave>();
    }

    public void GetScore(Action onSuccess = null)
    {
        Managers.WebContents.ReqGetUserAccount(new ReqDtoGetUserAccount()
        {
            UserName = Managers.Game.UserInfo.UserId
        },
       (response) =>
       {
           Managers.Game.UserInfo.RecordScore = response.HighScore;
           Managers.Game.UserInfo.LatelyScore = response.LatelyScore;
           onSuccess?.Invoke();
       },
       (errorCode) =>
       {
            Managers.UI.ShowPopupUI<UI_ErrorPopup>();           
       });
    }
    
    public void SetScore(Action onSuccess = null)
    {
        Managers.WebContents.InsertUserAccountScore(new ReqDtoInsertUserAccountScore()
        {
            UserName = Managers.Game.UserInfo.UserId,
            Score = Managers.Game.UserInfo.LatelyScore
        },
       (response) =>
       {
            Debug.Log("score 입력");
            onSuccess?.Invoke();
       },
       (errorCode) =>
       {
            Debug.Log("score 입력 실패");
            //Managers.UI.ShowPopupUI<UI_ErrorPopup>();           
       });
    }
}