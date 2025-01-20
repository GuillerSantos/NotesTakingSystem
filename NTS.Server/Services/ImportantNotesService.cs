using Microsoft.EntityFrameworkCore;
using NTS.Server.Data;
using NTS.Server.Entities;
using NTS.Server.Services.Contracts;

namespace NTS.Server.Services
{
    public class ImportantNotesService : IImpotantNotesService
    {
        private readonly ApplicationDbContext dbContext;

        public ImportantNotesService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<bool> MarkNoteAsImportantAsync(Guid noteId, Guid userId)
        {
            try
            {
                var note = await dbContext.Notes.FindAsync(noteId);
                if (note == null || note.UserId != userId) return false;

                var importantNote = new ImportantNotes
                {
                    ImportantNoteId = Guid.NewGuid(),
                    Title = note.Title,
                    Content = note.Content,
                    NoteId = noteId,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                };

                dbContext.ImportantNotes.Add(importantNote);
                await dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception error)
            {
                throw new Exception($"Error Marking Note As Important: {error.Message}");
            }
        }

        public async Task RemoveByNoteIdAsync(Guid noteId)
        {
            try
            {
                var importantNote = await dbContext.ImportantNotes
                    .Where(i => i.NoteId == noteId)
                    .ToListAsync();

                if (importantNote.Any())
                {
                    dbContext.ImportantNotes.RemoveRange(importantNote);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception error)
            {
                throw new Exception($"Error Removing Note: {error.Message}");
            }
        }
    }
}
