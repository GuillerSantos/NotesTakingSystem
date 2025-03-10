using Microsoft.EntityFrameworkCore;
using NTS.Server.Data;
using NTS.Server.Entities;
using NTS.Server.Services.Contracts;

namespace NTS.Server.Services
{
    public class CommentsService : ICommentsService
    {
        #region Fields

        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<CommentsService> logger;

        #endregion Fields

        #region Public Constructors

        public CommentsService(ApplicationDbContext dbContext, ILogger<CommentsService> logger)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task SaveCommentAsync(Comment comment)
        {
            dbContext.Comments.Add(comment);
            await dbContext.SaveChangesAsync();
        }

        public async Task<List<Comment>> GetCommentsForNoteAsync(Guid noteId)
        {
            return await dbContext.Comments
               .Where(c => c.NoteId == noteId)
               .OrderBy(c => c.CreatedAt)
               .ToListAsync();
        }

        #endregion Public Methods
    }
}