using GameApi.Dtos;
using System;
using System.Collections;
using System.Net.Http;
using UnityEngine;

public class WebRoute
{
    private readonly static string BaseUrl = $"https://localhost:7226/";
    public readonly static Func<ReqDtoGetUserAccount, string> GetUserAccount = (dto) => $"{BaseUrl}User/GetUserAccount?UserName={dto.UserName}&Password={dto.Password}";
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


