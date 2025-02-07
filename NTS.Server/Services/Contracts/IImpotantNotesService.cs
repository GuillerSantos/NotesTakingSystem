using Microsoft.EntityFrameworkCore.Storage;
using NTS.Server.Entities;

namespace NTS.Server.Services.Contracts
{
    public interface IImpotantNotesService
    {
        Task<bool> MarkNoteAsImportantAsync(Guid noteId, Guid userId);
        Task<List<ImportantNotes>> GetAllImportantNotesAsync(Guid userId);
        Task UnmarkNoteAsImportantAsync(Guid noteId);
        Task UpdateImportantNotesAsync(Notes updatedNote);
    }
}
