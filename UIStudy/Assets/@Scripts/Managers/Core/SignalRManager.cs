using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using UnityEditor.MemoryProfiler;

public class SignalRManager
{
    private HubConnection _connection;
    private string _serverUrl = "https://dev-single-api.snapism.net:8082/Chat";

    // 메세지를 받는 것
    // 메세지를 특정인물한테 보내는것 (친구 기능)
    // 메세지를 전체한테 보내는 것

    public void Init()
    {
        // SignalR 연결 생성
        _connection = new HubConnectionBuilder()
            .WithUrl(_serverUrl)  // 서버 URL 지정
            .WithAutomaticReconnect() // 자동 재연결
            .Build();

        OnRecieveMessage();
    }

    public async void Destroy()
    {
        if (_connection != null)
        {
            await _connection.StopAsync();
            await _connection.DisposeAsync();
        }
    }
    
    public void OnRecieveMessage()
    {
        _connection.On<string, string>("ReceiveMessage", (user, message) =>
        {
            //이벤트 호출 or ChatManager한테 보내주던지
            Debug.Log($"[{user}] {message}");
            //Managers.Event.TriggerEvent
        });
    }

    public async void SendMessageOneToOne(int callerUserId, int senderUserId, string message)
    {
        await _connection.InvokeAsync("SendMessageOneToOne", callerUserId, senderUserId, message);
    }

    public async void SendMessageAll(int userId, string message)
    {
        await _connection.InvokeAsync("SendMessageAll", userId, message);
    }
}