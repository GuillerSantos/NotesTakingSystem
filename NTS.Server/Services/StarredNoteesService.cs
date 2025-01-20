using Microsoft.EntityFrameworkCore;
using NTS.Server.Data;
using NTS.Server.Entities;
using NTS.Server.Services.Contracts;

namespace NTS.Server.Services
{
    public class StarredNoteesService : IStarredNotesService
    {
        private readonly ApplicationDbContext dbContext;

        public StarredNoteesService (ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> MarkNoteAsStarredAsync(Guid noteId, Guid userId)
        {
            try
            {
                var note = await dbContext.Notes.FindAsync(noteId);
                if (note == null || note.UserId != userId) return false;

                var starredNote = new StarredNotes
                {
                    StarredNotesId = Guid.NewGuid(),
                    NoteId = noteId,
                    UserId = userId,
                    Title = note.Title,
                    Content = note.Content,
                    CreatedAt = DateTime.UtcNow
                };

                dbContext.StarredNotes.Add(starredNote);
                await dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception error)
            {
                throw new Exception($"Error Marking Note As Starred: {error.Message}");
            }
        }

        public async Task RemoveByNoteIdAsync(Guid noteId)
        {
            try
            {
                var starredNote = await dbContext.StarredNotes
                    .Where(s => s.NoteId == noteId)
                    .ToListAsync();

                if (starredNote.Any())
                {
                    dbContext.StarredNotes.RemoveRange(starredNote);
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
