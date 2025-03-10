using Microsoft.EntityFrameworkCore;
using NTS.Server.Data;
using NTS.Server.Entities;
using NTS.Server.Services.Contracts;

namespace NTS.Server.Services
{
    public class SharedNotesService : ISharedNotesService
    {
        #region Fields

        private readonly ApplicationDbContext dbContext;

        #endregion Fields

        #region Public Constructors

        public SharedNotesService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<bool> MarkNoteAsSharedAsync(Guid noteId, Guid userId)
        {
            var note = await dbContext.Notes.FindAsync(noteId);
            if (note == null || note.UserId != userId) return false;

            var sharedNote = new SharedNotes
            {
                FullName = note.FullName,
                NoteId = noteId,
                UserId = userId,
                Title = note.Title,
                Content = note.Content,
                CreatedAt = DateTime.UtcNow,
                Color = note.Color
            };

            dbContext.SharedNotes.Add(sharedNote);
            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task UnmarkNoteAsSharedAsync(Guid noteId)
        {
            var sharedNotes = await dbContext.SharedNotes
                .Where(s => s.NoteId == noteId)
                .ExecuteDeleteAsync();
        }

        public async Task<List<SharedNotes>> GetAllSharedNotesAsync()
        {
            return await dbContext.SharedNotes
                .ToListAsync();
        }

        public async Task UpdateSharedNotesAsync(Notes updatedNote)
        {
            var sharedNotes = await dbContext.SharedNotes
                .Where(s => s.NoteId == updatedNote.NoteId)
                .ToListAsync();

            foreach (var sharedNote in sharedNotes)
            {
                sharedNote.Title = updatedNote.Title;
                sharedNote.Content = updatedNote.Content;
                sharedNote.Color = updatedNote.Color;
            }

            dbContext.SharedNotes.UpdateRange(sharedNotes);
            await dbContext.SaveChangesAsync();
        }

        #endregion Public Methods
    }
}