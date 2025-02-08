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

        public event Action<Guid, Guid, Guid, string, string, DateTime> OnCommentReceived = delegate { };

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

            hubConnection.On<Guid, Guid, Guid, string, string, DateTime>("ReceiveComment", (noteId, userId, sharedNoteId, fullName, commentContent, createdAt) =>
            {
                OnCommentReceived?.Invoke(noteId, userId, sharedNoteId, fullName, commentContent, createdAt);
            });
        }

        public async Task StopAsync()
        {
            await hubConnection.StopAsync();
        }

        public async Task SendCommentAsync(Guid noteId, Guid userId, Guid sharedNoteId, string fullName, DateTime createdAt, string commentContent)
        {
            await hubConnection.SendAsync("SendComment", noteId, userId, sharedNoteId, fullName, createdAt, commentContent);
        }
    }
}