using NTS.Server.Entities;

namespace NTS.Server.Services.Contracts
{
    public interface ISharedNotesService
    {
        Task<bool> MarkNoteAsSharedAsync(Guid noteId, Guid userId);
        Task<List<SharedNotes>> GetAllSharedNotesAsync();
        Task UnmarkNoteAsSharedAsync(Guid noteId);
        Task UpdateSharedNotesAsync(Notes updatedNote);
    }
}
