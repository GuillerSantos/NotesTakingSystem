using NTS.Client.Models;

namespace NTS.Client.Services.Contracts
{
    public interface IImportantNotesService
    {
        #region Public Methods

        Task MarkNoteAsImportantAsync(ImportantNotes request, Guid noteId);

        Task<List<ImportantNotes>> GetAllImportantNotesAsync();

        Task UnmarkNoteAsImportantAsync(Guid noteId);

        #endregion Public Methods
    }
}