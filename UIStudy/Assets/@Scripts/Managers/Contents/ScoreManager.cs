using UnityEngine;
using Newtonsoft.Json;
using System.Net.Http;
using GameApiDto.Dtos;

public class ScoreManager
{
    ScoreManagerSlave _slave = null;
    public void Init()
    {
        GameObject newObj = new GameObject("@ScoreManagerSlave");
        _slave = newObj.GetOrAddComponent<ScoreManagerSlave>();
    }

    public async void InsertScore(string userName, int score)
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, WebRoute.ReqInsertUserAccountScore);
        ReqInsertUserAccountScore requestDto = new ReqInsertUserAccountScore();
        requestDto.UserName = userName;
        requestDto.Score = score;
        string json = JsonConvert.SerializeObject(requestDto);
        var content = new StringContent(json, null, "application/json");
        request.Content = content;
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    public int GetScore(string userName, Define.EScoreType scoreType)
    {
/*
        int score = 0;
        _slave.GetScore(userName, scoreType, (result) =>
        {
            score = result;
            Debug.Log($"result : {result} score : {score}");
        });
*/
        return _slave.GetScore(userName, scoreType);
    }
    
    public void SetScore(string userName, int score)
    {
        _slave.SetScore(userName, score);
    }
}