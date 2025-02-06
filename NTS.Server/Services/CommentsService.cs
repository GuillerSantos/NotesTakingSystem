using NTS.Server.Data;
using NTS.Server.Entities;
using Microsoft.EntityFrameworkCore;
using NTS.Server.Services.Contracts;

namespace NTS.Server.Services
{
    public class CommentsService : ICommentsService
    {
        private readonly ApplicationDbContext dbContext;

        public CommentsService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


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
    }
}
