using Microsoft.EntityFrameworkCore;
using NTS.Server.Data;
using NTS.Server.Entities;
using NTS.Server.Services.Contracts;

namespace NTS.Server.Services
{
    public class StarredNoteesService : IStarredNotesService
    {
        private readonly ApplicationDbContext dbContext;

        public StarredNoteesService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
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
                    CreatedAt = DateTime.UtcNow,
                    Color = note.Color
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


        public async Task<List<StarredNotes>> GetAllStarredNotesAsync(Guid userId)
        {
            try
            {
                return await dbContext.StarredNotes
                    .Where(s => s.UserId == userId)
                    .ToListAsync();
            }
            catch (Exception error)
            {
                throw new Exception($"Error Fetching All Starred Notes: {error.Message}");
            }
        }

        public async Task<bool> UnmarkNoteAsStarredAsync(Guid noteId)
        {
            try
            {
                var starredNote = await dbContext.StarredNotes
                    .FirstOrDefaultAsync(s => s.NoteId == noteId);

                if (starredNote is null) return false;

                dbContext.StarredNotes.RemoveRange(starredNote);
                await dbContext.SaveChangesAsync();
                return true;


            }
            catch (Exception error)
            {
                throw new Exception($"Error Removing Note: {error.Message}");
            }
        }
    }
}
