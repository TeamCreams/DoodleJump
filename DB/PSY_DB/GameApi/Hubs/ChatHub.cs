using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using PSY_DB;
using PSY_DB.Tables;
using WebApi.Models.Dto;

namespace GameApi.Hubs
{
    
    public class ChatHub : Hub
    {
        private readonly PsyDbContext _dbContext;
        public ChatHub(PsyDbContext dbContext) 
        {
            _dbContext = dbContext;
        }


        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        
        public async Task SendMessageOneToOne(int callerUserId, int senderUserId, string message)
        {
            // 보내는 계정 찾기
            var callerUser = await _dbContext.TblUserAccounts
                                    .FirstOrDefaultAsync(user => user.Id == callerUserId && user.DeletedDate == null);
            if (callerUser == null)
            {
                throw new CommonException(EStatusCode.NotFoundEntity,
                    $"{callerUserId} : 찾을 수 없는 UserAccountId");
            }
            // 받는 계정 찾기
            var senderUser = await _dbContext.TblUserAccounts
                                    .FirstOrDefaultAsync(user => user.Id == senderUserId && user.DeletedDate == null);
            if (senderUser == null)
            {
                throw new CommonException(EStatusCode.NotFoundEntity,
                    $"{senderUserId} : 찾을 수 없는 UserAccountId");
            }

            // 메세지
            var userMessage = new TblUserMessage
            {
                UserAccountId = callerUser.Id,
                Message = message,
                MessageSentTime = DateTime.UtcNow,
                ReceiverUserId = senderUser.Id
            };
            _dbContext.TblUserMessages.Add(userMessage);
            var IsSuccess = await _dbContext.SaveChangesAsync();
            if (IsSuccess == 0)
            {
                throw new CommonException(EStatusCode.ChangedRowsIsZero,
                    $"UserAccountId : {callerUser.Id}의 메세지가 저장되지 않음.");
            }
            await Clients.User(senderUserId.ToString()).SendAsync("ReceiveMessage", callerUser.Id, message);
        }

        public async Task SendMessageAll(int userId, string message)
        {
            // 보내는 계정 찾기
            var callerUser = await _dbContext.TblUserAccounts
                                    .FirstOrDefaultAsync(user => user.Id == userId && user.DeletedDate == null);
            if (callerUser == null)
            {
                throw new CommonException(EStatusCode.NotFoundEntity,
                    $"{userId} : 찾을 수 없는 UserAccountId");
            }

            // 메세지
            var userMessage = new TblUserMessage
            {
                UserAccountId = callerUser.Id,
                Message = message,
                MessageSentTime = DateTime.UtcNow
            };
            _dbContext.TblUserMessages.Add(userMessage);
            var IsSuccess = await _dbContext.SaveChangesAsync();
            if (IsSuccess == 0)
            {
                throw new CommonException(EStatusCode.ChangedRowsIsZero,
                    $"UserAccountId : {callerUser.Id}의 메세지가 저장되지 않음.");
            }
            await Clients.All.SendAsync("ReceiveMessage", callerUser.Id, message);
        }
    }
}
