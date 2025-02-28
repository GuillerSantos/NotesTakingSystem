using Microsoft.AspNetCore.SignalR.Client;
using NTS.Client.Services.Contracts;

namespace NTS.Client.Services
{
    public class CommentSignalRService : ICommentSignalRService
    {
        private readonly HubConnection hubConnection;
        private readonly ILogger<CommentSignalRService> logger;

        public CommentSignalRService(HubConnection hubConnection, ILogger<CommentSignalRService> logger)
        {
            this.hubConnection = hubConnection;
            this.logger = logger;

            // Set Default Event Handler To Prevent Null Reference
            OnCommentReceived = delegate { };

            // Setup Automatic Reconnection
            ConfigureReconnection();
        }

        public event Action<Guid, Guid, Guid, string, string, DateTime> OnCommentReceived;

        /// <summary>
        /// Starts the SignalR connection and sets up event listeners.
        /// </summary>
        public async Task StartAsync()
        {
            try
            {
                if (hubConnection.State == HubConnectionState.Disconnected)
                {
                    await hubConnection.StartAsync();
                    logger.LogInformation("SignalR connection established.");
                }

                hubConnection.On<Guid, Guid, Guid, string, string, DateTime>("ReceiveComment", (noteId, userId, sharedNoteId, fullName, commentContent, createdAt) =>
                {
                    OnCommentReceived?.Invoke(noteId, userId, sharedNoteId, fullName, commentContent, createdAt);
                });
            }
            catch (Exception ex)
            {
                logger.LogError($"Error establishing SignalR connection: {ex.Message}");
            }
        }

        /// <summary>
        /// Stops the SignalR connection.
        /// </summary>
        public async Task StopAsync()
        {
            if (hubConnection.State != HubConnectionState.Disconnected)
            {
                await hubConnection.StopAsync();
                logger.LogInformation("SignalR connection stopped.");
            }
        }

        /// <summary>
        /// Ensures the SignalR connection is active before sending a comment.
        /// </summary>
        public async Task SendCommentAsync(Guid noteId, Guid userId, Guid sharedNoteId, string fullName, DateTime createdAt, string commentContent)
        {
            if (hubConnection.State != HubConnectionState.Connected)
            {
                logger.LogWarning("SignalR connection is not active. Attempting to reconnect...");
                await StartAsync();
            }

            if (hubConnection.State == HubConnectionState.Connected)
            {
                await hubConnection.SendAsync("SendComment", noteId, userId, sharedNoteId, fullName, createdAt, commentContent);
            }
            else
            {
                logger.LogError("Failed to send comment: SignalR connection is still inactive.");
            }
        }

        /// <summary>
        /// Configures automatic reconnection logic.
        /// </summary>
        private void ConfigureReconnection()
        {
            hubConnection.Closed += async (error) =>
            {
                logger.LogWarning($"SignalR connection closed. Reason: {error?.Message}");
                await Task.Delay(5000); // Wait before reconnecting
                await StartAsync();
            };

            hubConnection.Reconnecting += (error) =>
            {
                logger.LogWarning($"Reconnecting to SignalR: {error?.Message}");
                return Task.CompletedTask;
            };

            hubConnection.Reconnected += (connectionId) =>
            {
                logger.LogInformation($"Reconnected to SignalR. Connection ID: {connectionId}");
                return Task.CompletedTask;
            };
        }
    }
}