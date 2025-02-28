using Microsoft.AspNetCore.SignalR;
using NTS.Server.Entities;
using NTS.Server.Services.Contracts;

namespace NTS.Server.Middleware.Hubs
{
    public class CommentHub : Hub
    {
        private readonly ICommentsService _commentsService;

        public CommentHub(ICommentsService commentsService)
        {
            _commentsService = commentsService ?? throw new ArgumentNullException(nameof(commentsService));
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

            await _commentsService.SaveCommentAsync(comment);

            // Broadcast comment to all clients
            await Clients.All.SendAsync("ReceiveMessage", comment);
        }

        public async Task GetCommentsAsync(Guid noteId)
        {
            try
            {
                var comments = await _commentsService.GetCommentsForNoteAsync(noteId);
                await Clients.Caller.SendAsync("ReceiveComments", comments);
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("Error", $"Error fetching comments: {ex.Message}");
            }
        }
    }
}