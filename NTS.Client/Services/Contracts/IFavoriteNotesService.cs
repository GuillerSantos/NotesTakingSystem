using NTS.Client.Models;

namespace NTS.Client.Services.Contracts
{
    public interface IFavoriteNotesService
    {
        Task MarkAsFavoriteNoteAsync(FavoriteNotes request, Guid noteId);
        Task GetAllFavoriteNotesAsync();
        Task UnmarkNoteAsFavoriteNoteAsync(Guid noteId, Guid userId);
    }
}