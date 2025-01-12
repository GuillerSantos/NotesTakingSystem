using NTS.Server.Domain.DTOs;
using NTS.Server.Domain.Entities;

namespace NTS.Server.Services.Contracts
{
    public interface INotesService
    {
        Task<Notes> GetNotesByIdAsync(Guid noteId);
        Task<Notes?> CreateNoteAsync(NotesDto note, Guid userId);
        Task UpdateNoteAsync(NotesDto note);
        Task<bool> RemoveNoteAsync(Guid noteId);
    }
}