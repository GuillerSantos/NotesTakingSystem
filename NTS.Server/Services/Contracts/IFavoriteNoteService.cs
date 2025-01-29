using Microsoft.EntityFrameworkCore.Storage;
using NTS.Server.Entities;

namespace NTS.Server.Services.Contracts
{
    public interface IFavoriteNoteService
    {
        Task<bool> MarkNotesAsFavoriteAsync(Guid noteId, Guid userId);
        Task<List<FavoriteNotes>> GetAllFavoriteNotesAsync(Guid userId);
        Task<bool> UnmarkNoteAsFavoriteNoteAsync(Guid noteId, Guid userId);
    }
}
