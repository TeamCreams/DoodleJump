using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections;
using System.Net.Http;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class SignalRManager
{
    private HubConnection _connection;
    public Action<DateTime> OnChangedHeartBeat;

    private string _serverUrl = "https://dd37927.store/Chat";
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
        OnReceiveHeartBeat();

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
        _connection.On<string, string, bool>("ReceiveMessage", async (userNickname, message, isPrivateMessage) =>
        {
            // //이벤트 호출 or ChatManager한테 보내주던지
            ChattingStruct chattingStruct = new ChattingStruct(isPrivateMessage, message);
            Managers.Chatting.Event_SendMessage(chattingStruct);
            //Debug.Log($"SignalRManager :  [ {userNickname} ]  {message}, {isPrivateMessage}");
            await ReceiveMessageAsync(userNickname);
        });
    }
    public async void LoginUser(int userId)
    {
        await _connection.InvokeAsync("LoginUser", userId);
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
            await _connection.InvokeAsync("SendMessageAll", senderUserId, message);
        }
    }
    // public async Awaitable HandleReceiveMessage(string nickname, string message)
    // {
    //     Debug.Log($"HandleReceiveMessage [ {nickname} ]  {message}");
    //     Managers.Game.ChattingInfo.SenderNickname = nickname;
    //     await ReceiveMessageAsync(userNickname);
    // }
    private async Awaitable ReceiveMessageAsync(string nickname)
    {
        // 나는 메인스레드가 날 잡아줄때까지 기다릴거야.
        await Awaitable.MainThreadAsync();
        Managers.Game.ChattingInfo.SenderNickname = nickname;

        ChattingStruct chattingStruct = Managers.Chatting.GetChattingStruct();
        var bubble = Managers.UI.MakeSubItem<UI_ChattingItem>(parent: Managers.Game.ChattingInfo.Root);
        if (bubble == null)
        {
            Debug.Log("Bubble이 생성되지 않음");
        }
        else
        {
            bubble.SetInfo(chattingStruct);
        }
    }
    public void OnReceiveHeartBeat()
    {
        _connection.On<DateTime>("ReceiveHeartBeat", async (heartBeatData) =>
        {
            await Awaitable.MainThreadAsync();
            //Debug.Log($"ReceiveHeartBeat {heartBeatData}");
            await _connection.InvokeAsync("ReceiveHearBeatFromClient");
            OnChangedHeartBeat?.Invoke(heartBeatData);
        });
    }
}