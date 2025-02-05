using Microsoft.EntityFrameworkCore;
using NTS.Server.Data;
using NTS.Server.DTOs;
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
            catch (Exception error)
            {
                throw new Exception($"Error Marking Note {noteId} As Shared: {error.Message}", error);
            }
        }


        public async Task UnmarkNoteAsSharedAsync(Guid noteId)
        {
            try
            {
                var sharedNotes = await dbContext.SharedNotes
                    .Where(s => s.NoteId == noteId)
                    .ToListAsync();

                if (sharedNotes.Any())
                {
                    dbContext.SharedNotes.RemoveRange(sharedNotes);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception error)
            {
                throw new Exception($"Error Removing Note {noteId} from Shared: {error.Message}", error);
            }
        }


        public async Task<List<SharedNotes>> GetAllSharedNotesAsync(Guid userId)
        {
            try
            {
                return await dbContext.SharedNotes
                    .Where(s => s.UserId == userId)
                    .Include(s => s.Note)
                    .Include(s => s.User)
                    .ToListAsync();
            }
            catch (Exception error)
            {
                throw new Exception($"Error Fetching All Shared Notes for User {userId}: {error.Message}", error);
            }
        }


        public async Task<SharedNotes?> UpdateSharedNoteAsync(Guid noteId, Guid userId, SharedNoteUpdateDto request)
        {
            try
            {
                var sharedNote = await dbContext.SharedNotes
                    .FirstOrDefaultAsync(s => s.NoteId == noteId && s.UserId == userId);

                if (sharedNote == null)
                    return null;

                sharedNote.Title = request.Title;
                sharedNote.Content = request.Content;
                sharedNote.Color = request.Color;

                dbContext.SharedNotes.Update(sharedNote);
                await dbContext.SaveChangesAsync();

                return sharedNote;
            }
            catch (Exception error)
            {
                throw new Exception($"Error Updating Shared Note {noteId}: {error.Message}", error);
            }
        }
    }
}