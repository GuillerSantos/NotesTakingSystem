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

        public async Task SendCommentAsync(Guid noteId, Guid userId, string title, string fullName, DateTime createdAt, string content)
        {
            var comment = new Comment
            {
                NoteId = noteId,
                UserId = userId,
                Title = title,
                FullName = fullName,
                Content = content,
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
