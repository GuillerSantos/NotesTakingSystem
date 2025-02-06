using NTS.Client.Models;

namespace NTS.Client.Services.Contracts
{
    public interface ISharedNotesService
    {
        Task MarkNoteAsSharedAsync(SharedNotes request, Guid noteId);
        Task<List<SharedNotes>> GetAllSharedNotesAsync(Guid userId);
        Task UnmarkNoteAsSharedAsync(Guid noteId);
    }
}
