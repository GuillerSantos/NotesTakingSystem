using Microsoft.AspNetCore.SignalR;
using NTS.Server.Data;
using NTS.Server.Entities;
using NTS.Server.DTOs;
using Microsoft.EntityFrameworkCore;

namespace NTS.Server.Middleware.Hubs
{
    public class CommentHub : Hub
    {
        private readonly ApplicationDbContext dbContext;

        public CommentHub(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public async Task SendCommentAsync(Guid noteId, Guid sharedNoteId, Guid userId, string fullName, DateTime createdAt, string commentContent)
        {
            var comment = new Comment
            {
                NoteId = noteId,
                SharedNoteId = sharedNoteId,
                UserId = userId,
                FullName = fullName,
                CommentContent = commentContent,
                CreatedAt = createdAt
            };

            await dbContext.Comments.AddAsync(comment);
            await dbContext.SaveChangesAsync();

            // Broadcast comment to all clients
            await Clients.All.SendAsync("ReceiveMessage", comment);
        }


        public async Task GetCommentsAsync(Guid noteId)
        {
            try
            {
                var comments = await dbContext.Comments
                    .Where(c => c.NoteId == noteId)
                    .OrderBy(c => c.CreatedAt)
                    .ToListAsync();

                await Clients.Caller.SendAsync("ReceiveComments", comments);
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("Error", $"Error fetching comments: {ex.Message}");
            }
        }
    }
}
