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
                    .ExecuteDeleteAsync();
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
                    .ToListAsync();
            }
            catch (Exception error)
            {
                throw new Exception($"Error Fetching All Shared Notes for User {userId}: {error.Message}", error);
            }
        }

        public async Task UpdateSharedNotesAsync(Notes updatedNote)
        {
            try
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
            catch (Exception error)
            {
                throw new Exception($"Error Updating Shared Notes: {error.Message}");
            }
        }
    }
}