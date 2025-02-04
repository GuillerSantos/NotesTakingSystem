using NTS.Server.Data;
using NTS.Server.Entities;
using Microsoft.EntityFrameworkCore;
using NTS.Server.Services.Contracts;

namespace NTS.Server.Services
{
    public class CommentsService : ICommentsService
    {
        private readonly ApplicationDbContext _dbContext;

        public CommentsService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Comment>> GetCommentsForNoteAsync(Guid noteId)
        {
            return await _dbContext.Comments
                                    .Where(c => c.NoteId == noteId)
                                    .OrderBy(c => c.CreateAt)
                                    .ToListAsync();
        }
    }
}
