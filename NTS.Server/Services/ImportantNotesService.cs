using Microsoft.EntityFrameworkCore;
using NTS.Server.Data;
using NTS.Server.Entities;
using NTS.Server.Services.Contracts;

namespace NTS.Server.Services
{
    public class ImportantNotesService : IImpotantNotesService
    {
        #region Fields

        private readonly ApplicationDbContext dbContext;

        #endregion Fields

        #region Public Constructors

        public ImportantNotesService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<bool> MarkNoteAsImportantAsync(Guid noteId, Guid userId)
        {
            var isAlreadyMarked = await dbContext.ImportantNotes
                .AnyAsync(i => i.NoteId == noteId && i.UserId == userId);

            if (isAlreadyMarked)
            {
                Console.WriteLine("Note is already marked as important.");
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

        public async Task<List<ImportantNotes>> GetAllImportantNotesAsync(Guid userId)
        {
            return await dbContext.ImportantNotes
                .Where(i => i.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> UnmarkNoteAsImportantAsync(Guid noteId)
        {
            var importantNote = await dbContext.ImportantNotes
                .FirstOrDefaultAsync(i => i.NoteId == noteId);

            if (importantNote == null) return false;

            dbContext.ImportantNotes.Remove(importantNote);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task UpdateImportantNotesAsync(Notes updatedNote)
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

        #endregion Public Methods
    }
}