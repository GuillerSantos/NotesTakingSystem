using NTS.Client.Models;

namespace NTS.Client.Services.Contracts
{
    public interface ICommentsService
    {
        Task StartAsync();
        Task SendCommentAsync(Guid noteId, Guid userId, Guid sharedNoteId, string fullName, DateTime createdAt, string commentContent);
        Task<List<Comment>> GetCommentsForNoteAsync(Guid noteId);
        event Action<Guid, Guid, Guid, string, string, DateTime> OnCommentReceived;
    }
}
