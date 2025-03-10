using Microsoft.AspNetCore.Mvc;
using NTS.Server.Entities;
using NTS.Server.Services.Contracts;

namespace NTS.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        #region Fields

        private readonly ICommentsService commentsService;

        #endregion Fields

        #region Public Constructors

        public CommentsController(ICommentsService commentsService)
        {
            this.commentsService = commentsService ?? throw new ArgumentNullException(nameof(commentsService));
        }

        #endregion Public Constructors

        #region Public Methods

        [HttpPost("send-comment")]
        public async Task<ActionResult> SendCommentAsync([FromBody] Comment comment)
        {
            if (comment == null)
            {
                return BadRequest("Invalid comment data.");
            }

            await commentsService.SaveCommentAsync(comment);
            return Ok("Comment sent successfully.");
        }

        [HttpGet("get-comments/{noteId}")]
        public async Task<ActionResult<List<Comment>>> GetCommentsForNoteAsync(Guid noteId)
        {
            var comments = await commentsService.GetCommentsForNoteAsync(noteId);
            return comments is { Count: > 0 } ? Ok(comments) : NotFound("No comments found for this note.");
        }

        #endregion Public Methods
    }
}