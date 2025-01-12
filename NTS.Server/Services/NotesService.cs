using Microsoft.EntityFrameworkCore;
using NTS.Server.Database.DatabaseContext;
using NTS.Server.Domain.DTOs;
using NTS.Server.Domain.Entities;
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

        public async Task<Notes?> CreateNoteAsync(NotesDto request, Guid userId)
        {
            try
            {
                var note = new Notes 
                {
                    Title = request.Title,
                    Content = request.Content,
                    Priority = MapPriorityColorToPriorityString(request.Priority),
                    IsPublic = request.IsPublic,
                    CreatedAt = request.CreatedAt,
                    UserId = userId,
                };
                dbContext.Notes.Add(note);
                await dbContext.SaveChangesAsync();

                return note;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Creating Notes: {ex.Message}");
            }
        }


        public async Task<Notes> GetNotesByIdAsync(Guid noteId)
        {
            return await dbContext.Notes.FindAsync(noteId);
        }

        public async Task<bool> RemoveNoteAsync(Guid noteId)
        {
            var note = await dbContext.Notes.FindAsync(noteId);

            if (note == null)
            {
                return false;
            }

            dbContext.Notes.Remove(note);
            await dbContext.SaveChangesAsync();
            return true;
        }

        
        public Task UpdateNoteAsync(NotesDto note)
        {
            throw new NotImplementedException();
        }

        private string MapPriorityColorToPriorityString(string priorityColor)
        {
            switch (priorityColor.ToLower())
            {
                case "red":
                    return "High";
                case "yellow":
                    return "Low";
                case "green":
                    return "Normal";
                default:
                    return "Normal";
            }
        }
    }
}