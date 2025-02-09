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
                    NoteId = noteId,
                    UserId = userId,
                    FullName = note.FullName,
                    Title = note.Title,
                    Content = note.Content,
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


        public async Task<bool> UnmarkNoteAsImportantAsync(Guid noteId)
        {
            try
            {
                var importantNote = await dbContext.ImportantNotes
                    .FirstOrDefaultAsync(i => i.NoteId == noteId);

                if (importantNote == null) return false;

                dbContext.ImportantNotes.Remove(importantNote);
                await dbContext.SaveChangesAsync();
                return true;

            }
            catch (Exception error)
            {
                throw new Exception($"Error Removing Note: {error.Message}");
            }
        }


        public async Task UpdateImportantNotesAsync(Notes updatedNote)
        {
            try
            {
                var importantNotes = await dbContext.ImportantNotes
                    .Where(i => i.NoteId == updatedNote.NoteId)
                    .ToListAsync();

                foreach (var importantNote in importantNotes)
                {
                    importantNote.Title = updatedNote.Title;
                    importantNote.Content = updatedNote.Content;
                    importantNote.Color = updatedNote.Color;
                }

                dbContext.ImportantNotes.UpdateRange(importantNotes);
            }
            catch (Exception error)
            {
                throw new Exception($"Error Updating Important Notes: {error.Message}");
            }
        }
    }
}