using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NTS.Server.Data;
using NTS.Server.DTOs;
using NTS.Server.Entities;
using NTS.Server.Services.Contracts;

namespace NTS.Server.Services
{
    public class NotesService : INotesService
    {
        #region Fields

        private readonly ApplicationDbContext dbContext;
        private readonly IFavoriteNoteService favoriteNoteService;
        private readonly IImpotantNotesService importantNotesService;
        private readonly ISharedNotesService sharedNotesService;
        private readonly IStarredNotesService starredNotesService;

        #endregion Fields

        #region Public Constructors

        public NotesService(ApplicationDbContext dbContext, IFavoriteNoteService favoriteNoteService, IImpotantNotesService importantNotesService, ISharedNotesService sharedNotesService, IStarredNotesService starredNotesService)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.favoriteNoteService = favoriteNoteService ?? throw new ArgumentNullException(nameof(favoriteNoteService));
            this.importantNotesService = importantNotesService ?? throw new ArgumentNullException(nameof(importantNotesService));
            this.sharedNotesService = sharedNotesService ?? throw new ArgumentNullException(nameof(sharedNotesService));
            this.starredNotesService = starredNotesService ?? throw new ArgumentNullException(nameof(starredNotesService));
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<List<Notes>> GetAllNotesAsync(Guid userId)
        {
            return await dbContext.Notes
                .Where(n => n.UserId == userId)
                .ToListAsync();
        }

        public async Task<Notes> GetNoteByIdAsync(Guid noteId)
        {
            var fetchedNote = await dbContext.Notes.FindAsync(noteId);
            return fetchedNote!;
        }

        public async Task<List<Notes>> SearchNotesAsync(string? searchQuery, Guid userId)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return await dbContext.Notes
                    .Where(n => n.UserId == userId)
                    .ToListAsync();
            }
            var notes = await dbContext.Notes
                .Where(n => n.UserId == userId)
                .Where(n => EF.Functions.Like(n.Title, $"%{searchQuery}%"))
                .ToListAsync();

            return notes;
        }

        public async Task<Notes?> CreateNoteAsync([FromBody] CreateNotesDto noteDetails, Guid userId)
        {
            var user = await dbContext.ApplicationUsers.FindAsync(userId);
            if (user == null)
            {
                Console.WriteLine("User Not Found");
            }

            var newNote = new Notes
            {
                UserId = userId,
                FullName = user!.FullName,
                Title = noteDetails.Title,
                Content = noteDetails.Content,
                Color = noteDetails.Color,
                CreatedAt = DateTime.UtcNow
            };

            dbContext.Notes.Add(newNote);
            await dbContext.SaveChangesAsync();
            return newNote;
        }

        public async Task<Notes> UpdateNotesAsync(UpdateNotesDto updatedNoteDetails, Guid noteId, Guid userId)
        {
            var existingNote = await dbContext.Notes
                .Where(n => n.NoteId == noteId && n.UserId == userId)
                .FirstOrDefaultAsync();

            if (existingNote == null)
                return null!;

            existingNote.Title = updatedNoteDetails.Title;
            existingNote.Content = updatedNoteDetails.Content;
            existingNote.Color = updatedNoteDetails.Color;

            dbContext.Notes.Update(existingNote);

            await UpdateRelatedNotesTablesAsync(existingNote);

            await dbContext.SaveChangesAsync();

            return existingNote;
        }

        public async Task<bool> RemoveNoteAsync(Guid noteId, Guid userId)
        {
            var noteToRemove = await dbContext.Notes.FindAsync(noteId);
            if (noteToRemove is null)
                return false;

            await favoriteNoteService.UnmarkNoteAsFavoriteAsync(noteId);
            await importantNotesService.UnmarkNoteAsImportantAsync(noteId);
            await sharedNotesService.UnmarkNoteAsSharedAsync(noteId);
            await starredNotesService.UnmarkNoteAsStarredAsync(noteId);

            dbContext.Notes.Remove(noteToRemove);

            await dbContext.SaveChangesAsync();

            return true;
        }

        #endregion Public Methods

        #region Private Methods

        private async Task UpdateRelatedNotesTablesAsync(Notes updatedNote)
        {
            await favoriteNoteService.UpdateFavoriteNotesAsync(updatedNote);
            await sharedNotesService.UpdateSharedNotesAsync(updatedNote);
            await importantNotesService.UpdateImportantNotesAsync(updatedNote);
            await starredNotesService.UpdateStarredNotesAsync(updatedNote);
        }

        #endregion Private Methods
    }
}