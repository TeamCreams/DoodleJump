using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using PSY_DB;
using PSY_DB.Tables;
using System.Diagnostics;
using WebApi.Models.Dto;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace GameApi.Hubs
{

    public class ChatHub : Hub
    {
        private readonly PsyDbContext _dbContext;
        public ChatHub(PsyDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        // -> static을 붙임으로 모든 클라이언트가 같은 dictionary를 참조하게 됨
        private static Dictionary<int, string> _connectionIds = new Dictionary<int, string>();

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
            if (_connectionIds.ContainsKey(userAccountId))
            {
                _connectionIds[userAccountId] = Context.ConnectionId;
            }
            else
            {
                _connectionIds.Add(userAccountId, Context.ConnectionId);
            }
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("UserConnected", Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
            // dictionary에서 지울 userAccountId 찾은 후 해당 value 지우기.
            var removeKey = _connectionIds.FirstOrDefault(id => id.Value == Context.ConnectionId).Key;
            _connectionIds.Remove(removeKey);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
