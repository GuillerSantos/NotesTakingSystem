using Microsoft.EntityFrameworkCore;
using NTS.Server.Data;
using NTS.Server.Entities;
using NTS.Server.Services.Contracts;

namespace NTS.Server.Services
{
    public class FavoriteNoteService : IFavoriteNoteService
    {
        #region Fields

        private readonly ApplicationDbContext dbContext;

        #endregion Fields

        #region Public Constructors

        public FavoriteNoteService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<bool> MarkNotesAsFavoriteAsync(Guid noteId, Guid userId)
        {
            var isAlreadyMarked = await dbContext.FavoriteNotes
                .AnyAsync(f => f.NoteId == noteId && f.UserId == userId);

            if (isAlreadyMarked)
            {
                Console.WriteLine("Note Already Marked As Favorite");
                return false;
            }

            var note = await dbContext.Notes.FindAsync(noteId);
            if (note == null || note.UserId != userId) return false;

            var favoriteNote = new FavoriteNotes
            {
                FavoriteNoteId = Guid.NewGuid(),
                NoteId = noteId,
                UserId = userId,
                FullName = note.FullName,
                Title = note.Title,
                Content = note.Content,
                CreatedAt = DateTime.UtcNow,
                Color = note.Color
            };

            dbContext.FavoriteNotes.Add(favoriteNote);
            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<FavoriteNotes>> GetAllFavoriteNotesAsync(Guid userId)
        {
            return await dbContext.FavoriteNotes
                .Where(f => f.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> UnmarkNoteAsFavoriteAsync(Guid noteId)
        {
            var favoriteNote = await dbContext.FavoriteNotes
                .FirstOrDefaultAsync(f => f.NoteId == noteId);

            if (favoriteNote == null) return false;

            dbContext.FavoriteNotes.Remove(favoriteNote);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task UpdateFavoriteNotesAsync(Notes updatedNote)
        {
            var favoriteNotes = await dbContext.FavoriteNotes
                .Where(f => f.NoteId == updatedNote.NoteId)
                .ToListAsync();

            foreach (var favoriteNote in favoriteNotes)
            {
                favoriteNote.Title = updatedNote.Title;
                favoriteNote.Content = updatedNote.Content;
                favoriteNote.Color = updatedNote.Color;
            }

            dbContext.FavoriteNotes.UpdateRange(favoriteNotes);
        }

        #endregion Public Methods
    }
}