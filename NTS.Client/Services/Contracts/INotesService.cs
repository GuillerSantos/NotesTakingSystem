using NTS.Client.Models.DTOs;

namespace NTS.Client.Services.Contracts
{
    public interface INotesService
    {
        Task<List<NoteDto>> GetAllNotesAsync();
        Task CreateNoteAsync(NoteDto request);
        Task UpdateNoteAsync(NoteDto request, Guid noteId);
        Task RemoveNoteAsync(Guid noteId);
    }
}
