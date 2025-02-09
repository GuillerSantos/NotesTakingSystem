using NTS.Client.Models;

namespace NTS.Client.Services.Contracts
{
    public interface IStarredNotesService
    {
        Task MarkNoteAsStarredAsync(StarredNotes request, Guid noteId);
        Task<List<StarredNotes>> GetAllStarredNotesAsync();
        Task UnmarkNoteAsImportantNoteAsync(Guid noteId);
    }
}
