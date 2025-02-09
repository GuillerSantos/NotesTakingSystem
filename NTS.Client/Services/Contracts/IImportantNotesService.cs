using NTS.Client.Models;

namespace NTS.Client.Services.Contracts
{
    public interface IImportantNotesService
    {
        Task MarkNoteAsImportantAsync(ImportantNotes request, Guid noteId);
        Task<List<ImportantNotes>> GetAllImportantNotesAsync();
        Task UnamrkNoteAsImportantAsync(Guid noteId);
    }
}
