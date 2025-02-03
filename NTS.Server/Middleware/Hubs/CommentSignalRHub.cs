using Microsoft.AspNetCore.SignalR;

namespace NTS.Server.Middleware.Hubs
{
    public class CommentSignalRHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
