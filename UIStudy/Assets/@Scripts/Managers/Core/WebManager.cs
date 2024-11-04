using GameApi.Dtos;
using System;
using System.Collections;
using System.Net.Http;
using UnityEngine;

public class WebRoute
{
    private readonly static string BaseUrl = $"https://dev-single-api.snapism.net:8080/";
    public readonly static Func<ReqDtoGetUserAccount, string> GetUserAccount = (dto) => $"{BaseUrl}User/GetUserAccount?UserName={dto.UserName}";

    public readonly static Func<ReqDtoGetUserAccountId, string> GetUserAccountId = (dto) => $"{BaseUrl}User/GetUserAccountId?UserName={dto.UserName}";

    public readonly static string InsertUserAccount =  $"{BaseUrl}User/InsertUser";
    public readonly static string ReqInsertUserAccountScore = $"{BaseUrl}User/InsertUserAccountScore";
    //public readonly static Func<ReqInsertUserAccountScore, string> InsertUserAccountScore = (dto) => $"{BaseUrl}User/InsertUserAccountScore?UserName={dto.UserName}&Score{dto.Score}";

}

public class WebManager
{
    WebManagerSlave _slave;

    public void Init()
    {
        GameObject newObj = new GameObject("@WebManagerSlave");
        _slave = newObj.GetOrAddComponent<WebManagerSlave>();
    }

    public void SendGetRequest(string url, Action<string> callback = null)
    {
        _slave.SendGetRequest(url, callback);
    }

    public void SendPostRequest(string url, string body, Action<string> callback = null)
    {
        _slave.SendPostRequest(url, body, callback);
    }
}


