using NTS.Client.Models;

namespace NTS.Client.Services.Contracts
{
    public interface ICommentsService
    {
        Task<List<Comment>> GetCommentsForNoteAsync(Guid noteId);
    }
}
