using NTS.Client.Models;

namespace NTS.Client.Services.Contracts
{
    public interface IStarredNotesService
    {
        #region Public Methods

        Task MarkNoteAsStarredAsync(StarredNotes request, Guid noteId);

        Task<List<StarredNotes>> GetAllStarredNotesAsync();

        Task UnmarkNoteAsImportantNoteAsync(Guid noteId);

        #endregion Public Methods
    }
}