using Microsoft.AspNetCore.SignalR;
using NTS.Server.Entities;
using NTS.Server.Data;
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

        public async Task SendCommentAsync(Guid noteId, Guid userId, string title, string fullName, DateTime createdAt, string commentContent)
        {
            var comment = new Comment
            {
                NoteId = noteId,
                UserId = userId,
                Title = title,
                FullName = fullName,
                CommentContent = commentContent,
                CreateAt = createdAt
            };

            await dbContext.Comments.AddAsync(comment);
            await dbContext.SaveChangesAsync();

            // Broadcast comment to all connected clients
            await Clients.All.SendAsync("ReceiveMessage", noteId, userId, title, fullName, createdAt, commentContent);
        }


        public async Task GetCommentsAsync(Guid noteId)
        {
            try
            {
                var comments = await dbContext.Comments
                    .Where(c => c.NoteId == noteId)
                    .OrderBy(c => c.CreateAt)
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
