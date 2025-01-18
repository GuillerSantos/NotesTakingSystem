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


        public async Task<IEnumerable<Notes>> GetAllNotesAsync(Guid userId)
        {
            try
            {
                return await dbContext.Notes
                    .Where(note => note.UserId == userId)
                    .ToListAsync();
            }
            catch (Exception error)
            {
                throw new Exception($"Error Fetching All Notes: {error.Message}");
            }
        }


        public async Task<Notes> GetNoteByIdAsync(Guid noteId)
        {
            try
            {
                var fetchedNote = await dbContext.Notes.FindAsync(noteId);
                return fetchedNote!;
            }
            catch (Exception error)
            {
                throw new Exception($"Error Fetching Note By ID: {error.Message}");
            }
        }


        public async Task<IEnumerable<Notes>> SearchNotesAsync(string searchTerm, Guid userId)
        {
            try
            {
                return await dbContext.Notes
                    .Where(note => note.UserId == userId &&
                         (note.Title.Contains(searchTerm) || note.Content.Contains(searchTerm)))
                    .ToListAsync();
            }
            catch (Exception error)
            {
                throw new Exception($"Error Searching Notes: {error.Message}");
            }
        }


        public async Task<Notes?> CreateNoteAsync([FromBody] CreateNotesDto noteDetails, Guid userId)
        {
            try
            {
                var newNote = new Notes
                {
                    UserId = userId,
                    Title = noteDetails.Title,
                    Content = noteDetails.Content,
                    Priority = noteDetails.Priority,
                    CreatedAt = DateTime.UtcNow
                };

                dbContext.Notes.Add(newNote);
                await dbContext.SaveChangesAsync();
                return newNote;
            }
            catch (Exception error)
            {
                throw new Exception($"Error Creating Note: {error.Message}");
            }
        }


        public async Task<Notes> UpdateNotesAsync(UpdateNotesDto updatedNoteDetails, Guid noteId, Guid userId)
        {
            try
            {
                var existingNote = await dbContext.Notes.FindAsync(noteId);
                if (existingNote == null || existingNote.UserId != userId) return null;

                existingNote.Title = updatedNoteDetails.Title;
                existingNote.Content = updatedNoteDetails.Content;
                existingNote.Priority = updatedNoteDetails.Priority;

                dbContext.Notes.Update(existingNote);
                await dbContext.SaveChangesAsync();
                return existingNote;
            }
            catch (Exception error)
            {
                throw new Exception($"Error Editing Note: {error.Message}");
            }
        }


        public async Task<bool> RemoveNoteAsync(Guid noteId)
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                var noteToRemove = await dbContext.Notes.FindAsync(noteId);
                if (noteToRemove == null)
                    return false;

                var relatedRecordsToRemove = new List<IQueryable<object>>
                {
                     dbContext.FavoriteNotes.Where(favNote => favNote.NoteId == noteId),
                     dbContext.ImportantNotes.Where(impNote => impNote.NoteId == noteId),
                     dbContext.SharedNotes.Where(sharedNote => sharedNote.NoteId == noteId),
                     dbContext.StarredNotes.Where(starNote => starNote.NoteId == noteId)
                };

                foreach (var records in relatedRecordsToRemove)
                {
                    var recordsList = records.ToList();
                    dbContext.RemoveRange(records);
                }

                dbContext.Notes.Remove(noteToRemove);

                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception error)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error Removing Note: {error.Message}");
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
            catch (Exception error)
            {
                throw new Exception($"Error Marking Note As Favorite: {error.Message}");
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
            catch (Exception error)
            {
                throw new Exception($"Error Marking Note As Important: {error.Message}");
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
            catch (Exception error)
            {
                throw new Exception($"Error Marking Note As Shared: {error.Message}");
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
            catch (Exception error)
            {
                throw new Exception($"Error Marking Note As Starred: {error.Message}");
            }
        }
    }
}
