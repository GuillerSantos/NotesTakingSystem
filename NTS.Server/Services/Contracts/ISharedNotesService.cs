using NTS.Server.Entities;

namespace NTS.Server.Services.Contracts
{
    public interface ISharedNotesService
    {
        #region Public Methods

        Task<bool> MarkNoteAsSharedAsync(Guid noteId, Guid userId);

        Task<List<SharedNotes>> GetAllSharedNotesAsync();

        Task UnmarkNoteAsSharedAsync(Guid noteId);

        Task UpdateSharedNotesAsync(Notes updatedNote);

        #endregion Public Methods
    }
}