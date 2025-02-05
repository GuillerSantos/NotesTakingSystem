using Microsoft.AspNetCore.SignalR.Client;
using NTS.Client.Services.Contracts;
using System;
using System.Threading.Tasks;

namespace NTS.Client.Services
{
    public class CommentSignalRService : ICommentSignalRService
    {
        private readonly HubConnection hubConnection;

        public CommentSignalRService(HubConnection hubConnection)
        {
            this.hubConnection = hubConnection;
            OnCommentReceived = delegate { };
        }

        public event Action<Guid, Guid, string, string, string, DateTime> OnCommentReceived = delegate { };

        public async Task StartAsync()
        {
            try
            {
                await hubConnection.StartAsync();
                Console.WriteLine("SignalR connection established.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error establishing connection: {ex.Message}");
            }

            hubConnection.On<Guid, Guid, string, string, string, DateTime>("ReceiveComment", (noteId, userId, title, fullName, content, createdAt) =>
            {
                OnCommentReceived?.Invoke(noteId, userId, title, fullName, content, createdAt);
            });
        }

        public async Task StopAsync()
        {
            await hubConnection.StopAsync();
        }

        public async Task SendCommentAsync(Guid noteId, Guid userId, string title, string fullName, DateTime createdAt, string content)
        {
            await hubConnection.SendAsync("SendComment", noteId, userId, title, fullName, createdAt, content);
        }
    }
}