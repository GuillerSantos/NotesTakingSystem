using NTS.Client.Models;

namespace NTS.Client.Services.Contracts
{
    public interface IFavoriteNoteService
    {
        Task MarkAsFavoriteNoteAsync(FavoriteNotes request, Guid noteId);
    }
}
