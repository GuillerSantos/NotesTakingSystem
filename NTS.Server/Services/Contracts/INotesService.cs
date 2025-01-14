using NTS.Server.Entities;
using NTS.Server.Entities.DTOs;
using System.Threading.Tasks;

namespace NTS.Server.Services.Contracts
{
    public interface INotesService
    {
        Task<Notes?> CreateNoteAsync(NotesDto request, Guid userId);
        Task<Notes> EditNotesAsync(EditNotesDto editNotesDto, Guid noteId, Guid userId);
        Task<bool> RemoveNoteAsync(Guid noteId);
        Task<List<Notes>> GetAllNotesAsync(Guid userId);
        Task<IQueryable<Notes>> SearchNotesAsync(string searchTerm);
        Task<bool> MarkNoteAsFavoriteAsync(Guid noteId, Guid userId);
        Task<bool> MarkNoteAsImportantAsync(Guid noteId, Guid userId);
        Task<bool> MarkNoteAsSharedAsync(Guid noteId, Guid userId, Guid sharedWithUserId);
        Task<bool> MarkNoteAsStarredAsync(Guid noteId, Guid userId);
    }
}