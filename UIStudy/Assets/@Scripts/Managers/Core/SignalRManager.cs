﻿using Microsoft.AspNetCore.SignalR.Client;
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

    public async Task InitAsync()
    {
        // SignalR 연결 생성
        _connection = new HubConnectionBuilder()
            .WithUrl(_serverUrl)  // 서버 URL 지정
            .WithAutomaticReconnect() // 자동 재연결
            .Build();
        OnReceiveMessage();

        try
        {
            // 연결 시작
            await _connection.StartAsync();
            Debug.Log("SignalR 연결 성공!");
        }
        catch (Exception ex)
        {
            Debug.LogError($"SignalR 연결 실패: {ex.Message}");
        }
    }

    public async void Destroy()
    {
        if (_connection != null)
        {
            await _connection.StopAsync();
            await _connection.DisposeAsync();
        }
    }
    
    public void OnReceiveMessage()
    {
        _connection.On<string, string>("ReceiveMessage", (userId, message) =>
        {
            //이벤트 호출 or ChatManager한테 보내주던지
            Debug.Log($"SignalRManager :  [ {userId} ]  {message}");
            //Managers.Event.TriggerEvent
            ChattingStruct chattingStruct = new ChattingStruct(0, 0, "orange", message);
            Managers.Event.TriggerEvent(Define.EEventType.ReceiveMessage, null, chattingStruct);
        });
    }
    public void LoginUser(int userId)
    {
        userId = Managers.Game.UserInfo.UserAccountId;
        _connection.InvokeAsync("LoginUser", userId);
    }
    public async void SendMessageOneToOne(int senderUserId, int receiverUserId, string message)
    {
        if (_connection.State == HubConnectionState.Connected)
        {
            await _connection.InvokeAsync("SendMessageOneToOne", senderUserId, receiverUserId, message);
        }
   }

    public async void SendMessageAll(int senderUserId, string message)
    {
        if (_connection.State == HubConnectionState.Connected)
        {
            //await _connection.InvokeAsync("SendMessage", senderUserId.ToString(), message);
            await _connection.InvokeAsync("SendMessageAll", senderUserId, message);
        }
    }
}