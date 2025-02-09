using Microsoft.AspNetCore.SignalR.Client;
using NTS.Client.Services.Contracts;
using System;
using System.Threading.Tasks;

namespace NTS.Client.Services
{
    public class CommentSignalRService : ICommentSignalRService
    {
        private readonly HubConnection hubConnection;
        private readonly ILogger<CommentSignalRService> logger;

        public CommentSignalRService(HubConnection hubConnection, ILogger<CommentSignalRService> logger)
        {
            this.hubConnection = hubConnection;
            OnCommentReceived = delegate { };
            this.logger = logger;
        }

        public event Action<Guid, Guid, Guid, string, string, DateTime> OnCommentReceived = delegate { };

        public async Task StartAsync()
        {
            try
            {
                await hubConnection.StartAsync();
                logger.LogInformation("SignalR connection established.");
            }
            catch (Exception ex)
            {
                logger.LogError($"Error establishing connection: {ex.Message}");
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