namespace NTS.Client.Services.Contracts
{
    public interface ICommentSignalRService
    {
        event Action<Guid, Guid, Guid, string, string, DateTime> OnCommentReceived;

        Task StartAsync();
        Task StopAsync();
        Task SendCommentAsync(Guid noteId, Guid userId, Guid sharedNoteId, string fullName, DateTime createdAt, string commentContent);
    }
}