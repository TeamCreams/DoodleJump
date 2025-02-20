using GameApi.Dtos;
using System;
using System.Collections;
using System.Net.Http;
using System.Text;
using UnityEngine;



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
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("<color=green><b>Web Log Start : </b></color>");
        builder.AppendLine($" url : {url}");
        Debug.Log(builder.ToString());
        _slave.SendGetRequest(url, callback);
    }

    public void SendPostRequest(string url, string body, Action<string> callback = null)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("<color=green><b>Web Log Start : </b></color>");
        builder.AppendLine($" url : {url}");
        builder.AppendLine($" body : {body}");
        Debug.Log(builder.ToString());
        _slave.SendPostRequest(url, body, callback);
    }
}


