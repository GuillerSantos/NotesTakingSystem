using NTS.Server.Entities;

namespace NTS.Server.Services.Contracts
{
    public interface IFavoriteNoteService
    {
        Task<bool> MarkNotesAsFavoriteAsync(Guid noteId, Guid userId);
        Task<List<FavoriteNotes>> GetAllFavoriteNotesAsync(Guid userId);
        Task<bool> UnmarkNoteAsFavoriteAsync(Guid noteId);
        Task UpdateFavoriteNotesAsync(Notes updatedNote);
    }
}
