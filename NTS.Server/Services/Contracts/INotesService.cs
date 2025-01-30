using NTS.Server.Entities;
using NTS.Server.Entities.DTOs;
using System.Threading.Tasks;

namespace NTS.Server.Services.Contracts
{
    public interface INotesService
    {
        Task<Notes?> CreateNoteAsync(CreateNotesDto request, Guid userId);

        Task<Notes> UpdateNotesAsync(UpdateNotesDto editNotesDto, Guid noteId, Guid userId);

        Task<bool> RemoveNoteAsync(Guid noteId, Guid userId);

        Task<Notes> GetNoteByIdAsync(Guid noteId);

        Task<List<Notes>> GetAllNotesAsync(Guid userId);

        Task<List<Notes>> SearchNotesAsync(string searchQuery, Guid userId);
    }
}