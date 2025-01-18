using NTS.Server.Entities;
using NTS.Server.Entities.DTOs;
using System.Threading.Tasks;

namespace NTS.Server.Services.Contracts
{
    public interface INotesService
    {
        Task<Notes?> CreateNoteAsync(CreateNotesDto request, Guid userId);

        Task<Notes> UpdateNotesAsync(UpdateNotesDto editNotesDto, Guid noteId, Guid userId);

        Task<bool> RemoveNoteAsync(Guid noteId);

        Task<Notes> GetNoteByIdAsync(Guid noteId);

        Task<IEnumerable<Notes>> GetAllNotesAsync(Guid userId);

        Task<IEnumerable<Notes>> SearchNotesAsync(string searchTerm, Guid userId);

        Task<bool> MarkNoteAsFavoriteAsync(Guid noteId, Guid userId);

        Task<bool> MarkNoteAsImportantAsync(Guid noteId, Guid userId);

        Task<bool> MarkNoteAsSharedAsync(Guid noteId, Guid userId, Guid sharedWithUserId);

        Task<bool> MarkNoteAsStarredAsync(Guid noteId, Guid userId);
    }
}