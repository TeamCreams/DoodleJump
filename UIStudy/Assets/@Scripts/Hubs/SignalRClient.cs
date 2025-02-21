using System;
using System.Threading.Tasks;
using UnityEngine;
using Microsoft.AspNetCore.SignalR.Client;

public class SignalRClient : MonoBehaviour
{
    private HubConnection connection;

    async void Start()
    {
        await ConnectToSignalR();
    }

    private async Task ConnectToSignalR()
    {
        string serverUrl = "https://dev-single-api.snapism.net:8082/Chat";

        // SignalR 연결 생성
        connection = new HubConnectionBuilder()
            .WithUrl(serverUrl)  // 서버 URL 지정
            .WithAutomaticReconnect() // 자동 재연결
            .Build();

        // 서버에서 "ReceiveMessage" 이벤트가 발생하면 실행할 핸들러 추가
        connection.On<string, string>("ReceiveMessage", (user, message) =>
        {
            Debug.Log($"[{user}] {message}");
        });

        try
        {
            // 연결 시작
            await connection.StartAsync();
            Debug.Log("SignalR 연결 성공!");
        }
        catch (Exception ex)
        {
            Debug.LogError($"SignalR 연결 실패: {ex.Message}");
        }
    }

    public async void SendMessage(string user, string message)
    {
        if (connection != null && connection.State == HubConnectionState.Connected)
        {
            try
            {
                // 서버의 SendMessage 메서드 호출
                await connection.InvokeAsync("SendMessage", user, message);
            }
            catch (Exception ex)
            {
                Debug.LogError($"메시지 전송 실패: {ex.Message}");
            }
        }
    }

    private async void OnApplicationQuit()
    {
        if (connection != null)
        {
            await connection.StopAsync();
            await connection.DisposeAsync();
        }
    }
}
