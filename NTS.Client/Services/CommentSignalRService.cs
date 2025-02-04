using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using NTS.Client.Models;
using System.Net.Http;

namespace NTS.Client.Services
{
    public class CommentSignalRService : IAsyncDisposable
    {
        private readonly HubConnection hubConnection;

        public event Action<Guid, Guid, string, string, string, DateTime> OnCommentReceived;
        private Comment comment = new();

        public CommentSignalRService(NavigationManager navigationManager)
        {
            try
            {
                hubConnection = new HubConnectionBuilder()
                   .WithUrl(navigationManager.ToAbsoluteUri("/commenthub"))
                   .WithAutomaticReconnect()
                   .Build();

                // Handle incoming comment messages
                hubConnection.On<Guid, Guid, string, string, string, DateTime>("ReceiveMessage", (noteId, userId, title, fullName, commentContent, createAt) =>
                {
                    OnCommentReceived?.Invoke(noteId, userId, title, fullName, commentContent, createAt);
                });

                hubConnection.Closed += async (exception) =>
                {
                    Console.WriteLine($"Connection closed: {exception?.Message}");
                };

                hubConnection.Reconnecting += async (exception) =>
                {
                    Console.WriteLine($"Reconnect retry attempt due to: {exception?.Message}");
                    await Task.CompletedTask;
                };

                hubConnection.Reconnecting += async (retryCount) =>
                {
                    Console.WriteLine($"Reconnect retry attempt #{retryCount}");
                    await Task.CompletedTask;
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        public async Task StartAsync()
        {
            try
            {
                await hubConnection.StartAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Starting Connection: {ex.Message}");
            }
        }


        public async Task SendCommentAsync(Guid noteId, Guid userId, string title, string fullName, DateTime createdAt, string commentContent)
        {
            try
            {
                await hubConnection.SendAsync("SendCommentAsync", noteId, userId, title, fullName, commentContent, createdAt);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Sending Comment: {ex.Message}");
            }
        }


        public async ValueTask DisposeAsync()
        {
            await hubConnection.StopAsync();
            await hubConnection.DisposeAsync();
        }
    }
}
