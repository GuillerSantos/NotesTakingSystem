using NTS.Server.Entities;

namespace NTS.Server.Services.Contracts
{
    public interface IFavoriteNoteService
    {
        #region Public Methods

        Task<bool> MarkNotesAsFavoriteAsync(Guid noteId, Guid userId);

        Task<List<FavoriteNotes>> GetAllFavoriteNotesAsync(Guid userId);

        Task<bool> UnmarkNoteAsFavoriteAsync(Guid noteId);

        Task UpdateFavoriteNotesAsync(Notes updatedNote);

        #endregion Public Methods
    }
}