using Microsoft.AspNetCore.Mvc;
using NTS.Server.Entities;
using NTS.Server.Services;
using NTS.Server.Services.Contracts;

namespace NTS.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsService commentsService;

        public CommentsController(ICommentsService commentsService)
        {
            this.commentsService = commentsService;
        }

        [HttpGet("get-comments/{noteId}")]
        public async Task<ActionResult<List<Comment>>> GetCommentsForNoteAsync(Guid noteId)
        {
            var comments = await commentsService.GetCommentsForNoteAsync(noteId);
            if (comments == null || comments.Count == 0)
            {
                return NotFound("No comments found for this note.");
            }
            return Ok(comments);
        }
    }
}
