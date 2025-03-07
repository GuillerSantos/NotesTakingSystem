using NTS.Server.Entities;

namespace NTS.Server.Services.Contracts
{
    public interface ICommentsService
    {
        #region Public Methods

        Task SaveCommentAsync(Comment comment);

        Task<List<Comment>> GetCommentsForNoteAsync(Guid noteId);

        #endregion Public Methods
    }
}