using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using PSY_DB;
using PSY_DB.Tables;
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
        private Dictionary<int, string> _connectionIds = new Dictionary<int, string>();


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
            if (!_connectionIds[senderUserId].Contains(Context.ConnectionId))
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
            string receiverConnectionId = _connectionIds[receiverUserId];

            // 받아진 걸 확인 후 메세지가 보내졌다는 로그를 띄워야함.
            await Clients.User(receiverConnectionId).SendAsync("ReceiveMessage", Context.ConnectionId, message);
        }

        public async Task SendMessageAll(int senderUserId, string message)
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
            if (!_connectionIds[senderUserId].Contains(Context.ConnectionId))
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
            await Clients.All.SendAsync("ReceiveMessage", Context.ConnectionId, message);
        }

        public void LoginUser(int UserAccountId)
        {
            _connectionIds[UserAccountId] = Context.ConnectionId;
        }

        public override async Task OnConnectedAsync()
        {
            //await Clients.All.SendAsync("UserConnected", Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            //await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
            // dictionary에서 지울 userAccountId 찾은 후 해당 value 지우기.
            var removeKey = _connectionIds.FirstOrDefault(id => id.Value == Context.ConnectionId).Key;
            _connectionIds.Remove(removeKey);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
