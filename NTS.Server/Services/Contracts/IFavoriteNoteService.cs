using NTS.Server.Entities;

namespace NTS.Server.Services.Contracts
{
    public interface IFavoriteNoteService
    {
        Task<bool> MarkNotesAsFavoriteAsync(Guid noteId, Guid userId);

        Task RemoveByNoteIdAsync(Guid noteId);
    }
}
