using NTS.Client.Models;

namespace NTS.Client.Services.Contracts
{
    public interface ICommentsService
    {
        #region Events

        event Action<Guid, Guid, Guid, string, string, DateTime> OnCommentReceived;

        #endregion Events

        #region Public Methods

        Task StartAsync();

        Task SendCommentAsync(Guid noteId, Guid userId, Guid sharedNoteId, string fullName, DateTime createdAt, string commentContent);

        Task<List<Comment>> GetCommentsForNoteAsync(Guid noteId);

        #endregion Public Methods
    }
}