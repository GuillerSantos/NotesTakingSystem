using NTS.Server.Entities;

namespace NTS.Server.Services.Contracts
{
    public interface ICommentsService
    {
        Task<List<Comment>> GetCommentsForNoteAsync(Guid noteId);
    }
}
