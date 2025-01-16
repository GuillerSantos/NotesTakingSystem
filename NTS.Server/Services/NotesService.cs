using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NTS.Server.Database.DatabaseContext;
using NTS.Server.Entities;
using NTS.Server.Entities.DTOs;
using NTS.Server.Services.Contracts;

namespace NTS.Server.Services
{
    public class NotesService : INotesService
    {
        private readonly ApplicationDbContext dbContext;

        public NotesService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public async Task<Notes?> CreateNoteAsync([FromBody] CreateNotesDto request, Guid userId)
        {
            try
            {
                var note = new Notes
                {
                    UserId = userId,
                    Title = request.Title,
                    Content = request.Content,
                    Priority = request.Priority,
                    CreatedAt = DateTime.UtcNow
                };

                dbContext.Notes.Add(note);
                await dbContext.SaveChangesAsync();
                return note;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Creating Note: {ex.Message}");
            }
        }


        public async Task<Notes> EditNotesAsync(EditNotesDto editNotesDto, Guid noteId, Guid userId)
        {
            try
            {
                var existingNote = await dbContext.Notes.FindAsync(noteId);
                if (existingNote == null || existingNote.UserId != userId) return null;

                existingNote.Title = editNotesDto.Title;
                existingNote.Content = editNotesDto.Content;
                existingNote.Priority = editNotesDto.Priority;

                dbContext.Notes.Update(existingNote);
                await dbContext.SaveChangesAsync();
                return existingNote;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Editing Note: {ex.Message}");
            }
        }


        public async Task<bool> RemoveNoteAsync(Guid noteId)
        {
            try
            {
                var note = await dbContext.Notes.FindAsync(noteId);
                if (note == null) return false;

                dbContext.Remove(note);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Removing Note: {ex.Message}");
            }
        }


        public async Task<List<Notes>> GetAllNotesAsync(Guid userId)
        {
            try
            {
                return await dbContext.Notes.Where(n => n.UserId == userId).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Fetching All Notes: {ex.Message}");
            }
        }


        public async Task<Notes> GetNoteByIdAsync(Guid noteId)
        {
            try
            {
                var note = await dbContext.Notes.FindAsync(noteId);

                return note!;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Fetching Note By Note ID: {ex.Message}");
            }
        }


        public async Task<IQueryable<Notes>> SearchNotesAsync(string searchTerm, Guid userId)
        {
            try
            {
                var notes = dbContext.Notes
                    .Where(n => n.UserId == userId &&
                        (n.Title.Contains(searchTerm) || n.Content.Contains(searchTerm)));

                return notes;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Searching Notes: {ex.Message}");
            }
        }


        public async Task<bool> MarkNoteAsFavoriteAsync(Guid noteId, Guid userId)
        {
            try
            {
                var note = await dbContext.Notes.FindAsync(noteId);
                if (note == null || note.UserId != userId) return false;

                var favoriteNote = new FavoriteNotes
                {
                    FavoriteNoteId = Guid.NewGuid(),
                    Title = note.Title,
                    NoteId = noteId,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                };

                dbContext.FavoriteNotes.Add(favoriteNote);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Marking Note: {ex.Message}");
            }
        }


        public async Task<bool> MarkNoteAsImportantAsync(Guid noteId, Guid userId)
        {
            try
            {
                var note = await dbContext.Notes.FindAsync(noteId);
                if (note == null || note.UserId != userId) return false;

                var importantNote = new ImportantNotes
                {
                    ImportantNoteId = Guid.NewGuid(),
                    Title = note.Title,
                    NoteId = noteId,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                };

                dbContext.ImportantNotes.Add(importantNote);
                await dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Marking Note: {ex.Message}");
            }
        }


        public async Task<bool> MarkNoteAsSharedAsync(Guid noteId, Guid userId, Guid sharedWithUserId)
        {
            try
            {
                var note = await dbContext.Notes.FindAsync(noteId);
                if (note == null || note.UserId != userId) return false;

                var sharedNote = new SharedNotes
                {
                    SharedNoteId = Guid.NewGuid(),
                    Title = note.Title,
                    NoteId = noteId,
                    UserId = userId,
                    SharedWithUserId = sharedWithUserId,
                    CreatedAt = DateTime.UtcNow
                };

                dbContext.SharedNotes.Add(sharedNote);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Marking Note: {ex.Message}");
            }
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
                    Title = note.Title,
                    NoteId = noteId,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                };

                dbContext.StarredNotes.Add(starredNote);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Marking Note: {ex.Message}");
            }
        }
    }
}
