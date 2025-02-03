using Microsoft.EntityFrameworkCore.Storage;
using NTS.Server.Entities;

namespace NTS.Server.Services.Contracts
{
    public interface ISharedNotesService
    {
        Task<bool> MarkNoteAsSharedAsync(Guid noteId, Guid userId);
        Task<List<SharedNotes>> GetAllSharedNotesAsync(Guid userId);
        Task UnmarkNoteAsSharedAsync(Guid noteId);
    }
}
