using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NTS.Server.Data;
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

        public async Task<IEnumerable<NotesBase>> GetAllNotesAsync(Guid userId)
        {
            try
            {
                return await dbContext.NotesBase
                    .Where(note => note.UserId == userId)
                    .ToListAsync();
            }
            catch (Exception error)
            {
                throw new Exception($"Error fetching all notes: {error.Message}");
            }
        }

        public async Task<NotesBase> GetNoteByIdAsync(Guid noteId)
        {
            try
            {
                var note = await dbContext.NotesBase.FindAsync(noteId);
                return note!;
            }
            catch (Exception error)
            {
                throw new Exception($"Error fetching note by ID: {error.Message}");
            }
        }

        public async Task<IEnumerable<NotesBase>> SearchNotesAsync(string searchTerm, Guid userId)
        {
            try
            {
                return await dbContext.NotesBase
                    .Where(note => note.UserId == userId &&
                         (note.Title.Contains(searchTerm) || note.Content.Contains(searchTerm)))
                    .ToListAsync();
            }
            catch (Exception error)
            {
                throw new Exception($"Error searching notes: {error.Message}");
            }
        }

        public async Task<NotesBase?> CreateNoteAsync([FromBody] CreateNotesDto noteDetails, Guid userId)
        {
            try
            {
                var newNote = new NotesBase
                {
                    UserId = userId,
                    Title = noteDetails.Title,
                    Content = noteDetails.Content,
                    Priority = noteDetails.Priority,
                    CreatedAt = DateTime.UtcNow
                };

                dbContext.NotesBase.Add(newNote);
                await dbContext.SaveChangesAsync();
                return newNote;
            }
            catch (Exception error)
            {
                throw new Exception($"Error creating note: {error.Message}");
            }
        }

        public async Task<NotesBase> EditNotesAsync(EditNotesDto updatedNoteDetails, Guid noteId, Guid userId)
        {
            try
            {
                var existingNote = await dbContext.NotesBase.FindAsync(noteId);
                if (existingNote == null || existingNote.UserId != userId) return null;

                existingNote.Title = updatedNoteDetails.Title;
                existingNote.Content = updatedNoteDetails.Content;
                existingNote.Priority = updatedNoteDetails.Priority;

                dbContext.NotesBase.Update(existingNote);
                await dbContext.SaveChangesAsync();
                return existingNote;
            }
            catch (Exception error)
            {
                throw new Exception($"Error editing note: {error.Message}");
            }
        }

        public async Task<bool> RemoveNoteAsync(Guid noteId)
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                var noteToDelete = await dbContext.NotesBase.FindAsync(noteId);
                if (noteToDelete == null)
                    return false;

                var relatedRecordsToDelete = new List<IQueryable<object>>
                {
                     dbContext.FavoriteNotes.Where(favNote => favNote.NoteId == noteId),
                     dbContext.ImportantNotes.Where(impNote => impNote.NoteId == noteId),
                     dbContext.SharedNotes.Where(sharedNote => sharedNote.NoteId == noteId),
                     dbContext.StarredNotes.Where(starNote => starNote.NoteId == noteId)
                };

                foreach (var records in relatedRecordsToDelete)
                {
                    var recordsList = records.ToList();
                    dbContext.RemoveRange(records);
                }

                dbContext.NotesBase.Remove(noteToDelete);

                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception error)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error removing note: {error.Message}");
            }
        }

        public async Task<bool> MarkNoteAsFavoriteAsync(Guid noteId, Guid userId)
        {
            try
            {
                var note = await dbContext.NotesBase.FindAsync(noteId);
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
            catch (Exception error)
            {
                throw new Exception($"Error marking note as favorite: {error.Message}");
            }
        }

        public async Task<bool> MarkNoteAsImportantAsync(Guid noteId, Guid userId)
        {
            try
            {
                var note = await dbContext.NotesBase.FindAsync(noteId);
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
            catch (Exception error)
            {
                throw new Exception($"Error marking note as important: {error.Message}");
            }
        }

        public async Task<bool> MarkNoteAsSharedAsync(Guid noteId, Guid userId, Guid sharedWithUserId)
        {
            try
            {
                var note = await dbContext.NotesBase.FindAsync(noteId);
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
            catch (Exception error)
            {
                throw new Exception($"Error marking note as shared: {error.Message}");
            }
        }

        public async Task<bool> MarkNoteAsStarredAsync(Guid noteId, Guid userId)
        {
            try
            {
                var note = await dbContext.NotesBase.FindAsync(noteId);
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
            catch (Exception error)
            {
                throw new Exception($"Error marking note as starred: {error.Message}");
            }
        }
    }
}
