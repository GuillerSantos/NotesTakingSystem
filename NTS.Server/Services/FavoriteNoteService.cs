using Microsoft.EntityFrameworkCore;
using NTS.Server.Data;
using NTS.Server.Entities;
using NTS.Server.Services.Contracts;

namespace NTS.Server.Services
{
    public class FavoriteNoteService : IFavoriteNoteService
    {
        private readonly ApplicationDbContext dbContext;

        public FavoriteNoteService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public async Task<bool> MarkNotesAsFavoriteAsync(Guid noteId, Guid userId)
        {
            try
            {
                var note = await dbContext.Notes.FindAsync(noteId);
                if (note == null || note.UserId != userId) return false;

                var favoriteNote = new FavoriteNotes
                {
                    FavoriteNoteId = Guid.NewGuid(),
                    NoteId = noteId,
                    UserId = userId,
                    Title = note.Title,
                    Content = note.Content,
                    CreatedAt = DateTime.UtcNow,
                    Color = note.Color
                };

                dbContext.FavoriteNotes.Add(favoriteNote);
                await dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception error)
            {
                throw new Exception($"Error Marking Note As Favorite: {error.Message}");
            }
        }


        public async Task RemoveByNoteIdAsync(Guid noteId)
        {
            try
            {
                var favoriteNote = await dbContext.FavoriteNotes
                    .Where(f => f.NoteId == noteId)
                    .ToListAsync();

                if (favoriteNote.Any())
                {
                    dbContext.FavoriteNotes.RemoveRange(favoriteNote);
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
