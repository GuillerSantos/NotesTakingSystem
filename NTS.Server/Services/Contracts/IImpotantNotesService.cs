using Microsoft.EntityFrameworkCore.Storage;

namespace NTS.Server.Services.Contracts
{
    public interface IImpotantNotesService
    {
        Task<bool> MarkNoteAsImportantAsync(Guid noteId, Guid userId);

        Task RemoveByNoteIdAsync(Guid noteId);
    }
}
