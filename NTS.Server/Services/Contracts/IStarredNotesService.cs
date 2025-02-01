using Microsoft.EntityFrameworkCore.Storage;
using NTS.Server.Entities;

namespace NTS.Server.Services.Contracts
{
    public interface IStarredNotesService
    {
        Task<bool> MarkNoteAsStarredAsync(Guid noteId, Guid userId);
        Task<List<StarredNotes>> GetAllStarredNotesAsync(Guid userId);
        Task<bool> UnmarkNoteAsStarredAsync(Guid noteId);
    }
}
