using NTS.Server.Entities;

namespace NTS.Server.Services.Contracts
{
    public interface IStarredNotesService
    {
        #region Public Methods

        Task<bool> MarkNoteAsStarredAsync(Guid noteId, Guid userId);

        Task<List<StarredNotes>> GetAllStarredNotesAsync(Guid userId);

        Task<bool> UnmarkNoteAsStarredAsync(Guid noteId);

        Task UpdateStarredNotesAsync(Notes updatedNote);

        #endregion Public Methods
    }
}