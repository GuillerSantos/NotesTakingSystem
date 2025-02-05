using NTS.Client.Models;

namespace NTS.Client.Services.Contracts
{
    public interface ISharedNotesService
    {
        Task<List<SharedNotes>> GetAllSharedNotesAsync();
        Task<SharedNotes> GetSharedNoteByIdAsync(Guid noteId);
        Task MarkNoteAsSharedAsync(Guid noteId);     
        Task UpdateSharedNoteAsync(SharedNotes updatedNote);
        Task DeleteSharedNoteAsync(Guid noteId);
    }
}
