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
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        public async Task<bool> MarkNoteAsImportantAsync(Guid noteId, Guid userId)
        {
            try
            {
                var isAlreadyMarkred = await dbContext.ImportantNotes
                    .AnyAsync(i => i.NoteId == noteId && i.UserId == userId);

                if (isAlreadyMarkred)
                {
                    Console.WriteLine("Note Is Already Marked As Important");
                    return false;
                }

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
                    Color = note.Color
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


        public async Task<List<ImportantNotes>> GetAllImportantNotesAsync(Guid userId)
        {
            try
            {
                return await dbContext.ImportantNotes
                    .Where(i => i.UserId == userId)
                    .ToListAsync();
            }
            catch (Exception error)
            {
                throw new Exception($"Error Fetching Important Notes: {error.Message}");
            }
        }


        public async Task UnmarkNoteAsImportantAsync(Guid noteId)
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
