using NTS.Client.Models;
using NTS.Client.Services.Contracts;

namespace NTS.Client.Services
{
    public class CommentsService : ICommentsService
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<CommentsService> logger;

        public CommentsService(HttpClient httpClient, ILogger<CommentsService> logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }


        public event Action<Guid, Guid, Guid, string, string, DateTime>? OnCommentReceived;


        public async Task SendCommentAsync(Guid noteId, Guid userId, Guid sharedNoteId, string fullName, DateTime createdAt, string commentContent)
        {
            try
            {
                var comment = new Comment
                {
                    NoteId = noteId,
                    UserId = userId,
                    SharedNoteId = sharedNoteId,
                    FullName = fullName,
                    CreatedAt = createdAt,
                    CommentContent = commentContent
                };

                var response = await httpClient.PostAsJsonAsync("/api/Comments/send-comment", comment);
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    OnCommentReceived?.Invoke(noteId, userId, sharedNoteId, fullName, commentContent, createdAt);
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Error sending comment: {ex.Message}");
            }
        }


        public async Task<List<Comment>> GetCommentsForNoteAsync(Guid noteId)
        {
            try
            {
                var response = await httpClient.GetAsync($"/api/Comments/get-comments/{noteId}");
                response.EnsureSuccessStatusCode();

                var comments = await response.Content.ReadFromJsonAsync<List<Comment>>();
                return comments ?? new List<Comment>();
            }
            catch (HttpRequestException httpError)
            {
                logger.LogError($"HTTP Request Error: {httpError.Message}");
            }
            catch (Exception error)
            {
                logger.LogError($"Unexpected Error: {error.Message}");
            }

            return new List<Comment>();
        }


        public Task StartAsync()
        {
            throw new NotImplementedException("StartAsync logic is not yet implemented.");
        }
    }

}
