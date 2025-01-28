using Microsoft.EntityFrameworkCore.Storage;

namespace NTS.Server.Services.Contracts
{
    public interface ISharedNotesService
    {
        Task<bool> MarkNoteAsSharedAsync(Guid noteId, Guid userId);

        Task RemoveByNoteIdAsync(Guid noteId);
    }
}
