using Microsoft.EntityFrameworkCore;
using NTS.Server.Data;
using NTS.Server.Entities;
using NTS.Server.Services.Contracts;

namespace NTS.Server.Services
{
    public class CommentsService : ICommentsService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<CommentsService> logger;

        public CommentsService(ApplicationDbContext dbContext, ILogger<CommentsService> logger)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task SaveCommentAsync(Comment comment)
        {
            try
            {
                dbContext.Comments.Add(comment);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error saving comment: {ErrorMessage}", ex.Message);
                throw;
            }
        }

        public async Task<List<Comment>> GetCommentsForNoteAsync(Guid noteId)
        {
            try
            {
                return await dbContext.Comments
                   .Where(c => c.NoteId == noteId)
                   .OrderBy(c => c.CreatedAt)
                   .ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting comments for note {NoteId}: {ErrorMessage}", noteId, ex.Message);
                throw;
            }
        }
    }
}
