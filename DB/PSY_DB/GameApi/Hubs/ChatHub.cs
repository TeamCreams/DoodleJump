using GameApi.Controllers;
using GameApi.Dtos;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using PSY_DB;
using PSY_DB.Tables;
using System.Collections.Concurrent;
using System.Diagnostics;
using WebApi.Models.Dto;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace GameApi.Hubs
{

    public class ChatHub : Hub
    {
        private readonly PsyDbContext _dbContext;
        // -> static을 붙임으로 모든 클라이언트가 같은 dictionary를 참조하게 됨
        // -> ConcurrentDictionary 여러 스레드에서 수정해도 안전함
        // -> dictionary를 안 쓰면 한 계정에서 로그아웃하면 heartBeat가 다른 계정들에도 안 보내짐.
        private static ConcurrentDictionary<int, string> _connectionIds = new ConcurrentDictionary<int, string>();
        private static ConcurrentDictionary<string, CancellationTokenSource> _heartbeatTokens = new ConcurrentDictionary<string, CancellationTokenSource>();

        public ChatHub(PsyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendMessageOneToOne(int senderUserId, int receiverUserId, string message)
        {
            // 보내는 계정 찾기
            var senderUser = await _dbContext.TblUserAccounts
                                    .FirstOrDefaultAsync(user => user.Id == senderUserId && user.DeletedDate == null);
            if (senderUser == null)
            {
                throw new CommonException(EStatusCode.NotFoundEntity,
                    $"{senderUserId} : 찾을 수 없는 UserAccountId");
            }
            // 받는 계정 찾기
            var receiverUser = await _dbContext.TblUserAccounts
                                    .FirstOrDefaultAsync(user => user.Id == receiverUserId && user.DeletedDate == null);
            if (receiverUser == null)
            {
                throw new CommonException(EStatusCode.NotFoundEntity,
                    $"{receiverUserId} : 찾을 수 없는 UserAccountId");
            }

            // 네트워크에 연결 되어있지 않으면 throw
            if (!_connectionIds.TryGetValue(senderUserId, out var connectionIds) || !connectionIds.Contains(Context.ConnectionId))
            {
                throw new CommonException(EStatusCode.NotConnectionUser,
                        $"{Context.ConnectionId} : 연결 되어있지 않은 UserAccountId");
            }

            // 메세지
            var userMessage = new TblUserMessage
            {
                UserAccountId = senderUser.Id,
                Message = message,
                MessageSentTime = DateTime.UtcNow,
                ReceiverUserId = receiverUser.Id
            };

            _dbContext.TblUserMessages.Add(userMessage);
            var IsSuccess = await _dbContext.SaveChangesAsync();
            if (IsSuccess == 0)
            {
                throw new CommonException(EStatusCode.ChangedRowsIsZero,
                    $"UserAccountId : {senderUser.Id}의 메세지가 저장되지 않음.");
            }

            await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", senderUser.Nickname, message, true);

            if (!_connectionIds.ContainsKey(receiverUserId))
            {
                return;
            }
            string receiverConnectionId = _connectionIds[receiverUserId];
            // 받아진 걸 확인 후 메세지가 보내졌다는 로그를 띄워야함.
            await Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", senderUser.Nickname, message, true);
        }

        public async Task SendMessageAll(int senderUserId, string message)
        {
            try
            {
                // 보내는 계정 찾기
                var senderUser = await _dbContext.TblUserAccounts
                                        .FirstOrDefaultAsync(user => user.Id == senderUserId && user.DeletedDate == null);
                if (senderUser == null)
                {
                    throw new CommonException(EStatusCode.NotFoundEntity,
                        $"{senderUserId} : 찾을 수 없는 UserAccountId");
                }

                // 여기서 막힘
                //string a = $"SendMessageAll, id is {_connectionIds[senderUserId]}"
                //await Clients.All.SendAsync("ReceiveMessage", Context.ConnectionId, a);

                // 네트워크에 연결 되어있지 않으면 throw
                //if( 0 < _connectionIds.Count())// 이때는 됨. 제대로 추가가 되지 않는 거임
                if (!_connectionIds.TryGetValue(senderUserId, out var connectionIds) || !connectionIds.Contains(Context.ConnectionId))
                {
                    throw new CommonException(EStatusCode.NotConnectionUser,
                            $"{Context.ConnectionId} : 연결 되어있지 않은 UserAccountId");
                }

                // 메세지
                var userMessage = new TblUserMessage
                {
                    UserAccountId = senderUser.Id,
                    Message = message,
                    MessageSentTime = DateTime.UtcNow
                };
                _dbContext.TblUserMessages.Add(userMessage);
                var IsSuccess = await _dbContext.SaveChangesAsync();
                if (IsSuccess == 0)
                {
                    throw new CommonException(EStatusCode.ChangedRowsIsZero,
                        $"UserAccountId : {senderUser.Id}의 메세지가 저장되지 않음.");
                }
                await Clients.All.SendAsync("ReceiveMessage", senderUser.Nickname, message, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SendMessageAll: {ex.Message}");
                throw new CommonException(EStatusCode.UnknownError,
                        $"{ex.Message} 에러 발생");
            }
        }


        public async void LoginUser(int userAccountId)
        {
            _connectionIds.TryAdd(userAccountId, Context.ConnectionId);
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("UserConnected", Context.ConnectionId);
            await SendHeartBeat(); // HeartBeat 시작
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);

            // 연결 해제 시 HeartBeat 중지
            if (_heartbeatTokens.TryRemove(Context.ConnectionId, out var cts))
            {
                cts.Cancel();
                cts.Dispose();
            }

            var removeKey = _connectionIds.FirstOrDefault(id => id.Value == Context.ConnectionId).Key;
            _connectionIds.TryRemove(removeKey, out string message);
            await base.OnDisconnectedAsync(exception);
        }
        public async Task SendHeartBeat()
        {
            var serverTime = DateTime.UtcNow;
            await Clients.Client(Context.ConnectionId).SendAsync("ReceiveHeartBeat", serverTime);
        }

        public async Task ReceiveHearBeatFromClient()
        {
            await Task.Delay(5000);
            var serverTime = DateTime.UtcNow;
            await Clients.Client(Context.ConnectionId).SendAsync("ReceiveHeartBeat", serverTime);
        }

        public async Task SendHeartBeat1(int senderUserId)
        {
            // 보내는 계정 찾기
            var senderUser = await _dbContext.TblUserAccounts
                                    .FirstOrDefaultAsync(user => user.Id == senderUserId && user.DeletedDate == null);
            if (senderUser == null)
            {
                throw new CommonException(EStatusCode.NotFoundEntity,
                    $"{senderUserId} : 찾을 수 없는 UserAccountId");
            }

            // 네트워크에 연결 되어있지 않으면 throw
            if (!_connectionIds.TryGetValue(senderUserId, out var connectionIds) || !connectionIds.Contains(Context.ConnectionId))
            {
                throw new CommonException(EStatusCode.NotConnectionUser,
                        $"{Context.ConnectionId} : 연결 되어있지 않은 UserAccountId");
            }
            var serverTime = DateTime.UtcNow;
            await Clients.Client(Context.ConnectionId).SendAsync("ReceiveHeartBeat", serverTime);
            // await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", senderUser.Nickname, serverTime.ToString(), true);
        }
    }
}
