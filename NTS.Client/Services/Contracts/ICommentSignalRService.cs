using System;

namespace NTS.Client.Services.Contracts
{
    public interface ICommentSignalRService
    {
        event Action<Guid, Guid, string, string, string, DateTime> OnCommentReceived;

        Task StartAsync();
        Task StopAsync();
        Task SendCommentAsync(Guid noteId, Guid userId, string title, string fullName, DateTime createdAt, string content);
    }
}