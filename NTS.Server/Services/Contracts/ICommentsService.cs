using NTS.Server.Entities;

namespace NTS.Server.Services.Contracts
{
    public interface ICommentsService
    {
        Task SaveCommentAsync(Comment comment);
        Task<List<Comment>> GetCommentsForNoteAsync(Guid noteId);
    }
}