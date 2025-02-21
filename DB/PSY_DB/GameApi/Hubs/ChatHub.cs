using Microsoft.AspNetCore.SignalR;
using PSY_DB;

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
            // 어떻게 할건지.

        }

        public async Task SendMessageAll(int userId, string message)
        {
            // 어떻게 할건지.

        }

    }
}
