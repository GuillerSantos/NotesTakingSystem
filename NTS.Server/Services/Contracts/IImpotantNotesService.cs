using NTS.Server.Entities;

namespace NTS.Server.Services.Contracts
{
    public interface IImpotantNotesService
    {
        #region Public Methods

        Task<bool> MarkNoteAsImportantAsync(Guid noteId, Guid userId);

        Task<List<ImportantNotes>> GetAllImportantNotesAsync(Guid userId);

        Task<bool> UnmarkNoteAsImportantAsync(Guid noteId);

        Task UpdateImportantNotesAsync(Notes updatedNote);

        #endregion Public Methods
    }
}