using Microsoft.EntityFrameworkCore;
using NTS.Server.Data;
using NTS.Server.Entities;
using NTS.Server.Services.Contracts;

namespace NTS.Server.Services
{
    public class StarredNotesService : IStarredNotesService
    {
        #region Fields

        private readonly ApplicationDbContext dbContext;

        #endregion Fields

        #region Public Constructors

        public StarredNotesService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<bool> MarkNoteAsStarredAsync(Guid noteId, Guid userId)
        {
            var note = await dbContext.Notes.FindAsync(noteId);
            if (note == null || note.UserId != userId) return false;

            var starredNote = new StarredNotes
            {
                StarredNoteId = Guid.NewGuid(),
                NoteId = noteId,
                UserId = userId,
                FullName = note.FullName,
                Title = note.Title,
                Content = note.Content,
                CreatedAt = DateTime.UtcNow,
                Color = note.Color
            };

            dbContext.StarredNotes.Add(starredNote);
            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<StarredNotes>> GetAllStarredNotesAsync(Guid userId)
        {
            return await dbContext.StarredNotes
                .Where(s => s.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> UnmarkNoteAsStarredAsync(Guid noteId)
        {
            var starredNote = await dbContext.StarredNotes
                .FirstOrDefaultAsync(s => s.NoteId == noteId);

            if (starredNote is null) return false;

            dbContext.StarredNotes.RemoveRange(starredNote);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task UpdateStarredNotesAsync(Notes updatedNote)
        {
            var starredNotes = await dbContext.StarredNotes
                .Where(s => s.NoteId == updatedNote.NoteId)
                .ToListAsync();

            foreach (var starredNote in starredNotes)
            {
                starredNote.Title = updatedNote.Title;
                starredNote.Content = updatedNote.Content;
                starredNote.Color = updatedNote.Color;
            }

            dbContext.StarredNotes.UpdateRange(starredNotes);
        }

        #endregion Public Methods
    }
}