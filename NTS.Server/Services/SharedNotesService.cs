using Microsoft.EntityFrameworkCore;
using NTS.Server.Data;
using NTS.Server.Entities;
using NTS.Server.Services.Contracts;

namespace NTS.Server.Services
{
    public class SharedNotesService : ISharedNotesService
    {
        private readonly ApplicationDbContext dbContext;

        public SharedNotesService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<bool> MarkNoteAsSharedAsync(Guid noteId, Guid userId)
        {
            try
            {
                var note = await dbContext.Notes.FindAsync(noteId);
                if (note == null || note.UserId != userId) return false;

                var sharedNote = new SharedNotes
                {
                    SharedNoteId = Guid.NewGuid(),
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
            catch (Exception error)
            {
                throw new Exception($"Error Marking Note As Shared: {error.Message}");
            }
        }

        public async Task UnmarkNoteAsSharedAsync(Guid noteId)
        {
            try
            {
                var sharedNote = await dbContext.SharedNotes
                    .Where(s => s.NoteId == noteId)
                    .ToListAsync();

                if (sharedNote.Any())
                {
                    dbContext.SharedNotes.RemoveRange(sharedNote);
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
