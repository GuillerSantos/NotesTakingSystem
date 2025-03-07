using NTS.Client.Models;

namespace NTS.Client.Services.Contracts
{
    public interface IFavoriteNotesService
    {
        #region Public Methods

        Task MarkAsFavoriteNoteAsync(FavoriteNotes request, Guid noteId);

        Task UnmarkNoteAsFavoriteNoteAsync(Guid noteId);

        Task<List<FavoriteNotes>> GetAllFavoriteNotesAsync();

        #endregion Public Methods
    }
}