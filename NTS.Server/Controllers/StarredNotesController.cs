using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTS.Server.Services.Contracts;
using NTS.Server.Utilities;

namespace NTS.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StarredNotesController : ControllerBase
    {
        #region Fields

        private readonly IStarredNotesService starredNotesService;

        #endregion Fields

        #region Public Constructors

        public StarredNotesController(IStarredNotesService starredNotesService)
        {
            this.starredNotesService = starredNotesService ?? throw new ArgumentNullException(nameof(starredNotesService));
        }

        #endregion Public Constructors

        #region Public Methods

        [HttpPost("mark-starred/{noteId}"), Authorize(Roles = "DefaultUser")]
        public async Task<IActionResult> MarkNoteAsStarredAsync(Guid noteId)
        {
            if (!UserClaimUtil.TryGetUserId(User, out Guid userId))
            {
                return Unauthorized("User ID not found.");
            }

            var markedNote = await starredNotesService.MarkNoteAsStarredAsync(noteId, userId);
            if (!markedNote)
                return NotFound("Note Not Found Or You Are Not Authorized To Mark This Note As Starred");

            return Ok($"Note Marked Successfully: {markedNote}");
        }

        [HttpGet("get-all-starrednotes"), Authorize(Roles = "DefaultUser")]
        public async Task<IActionResult> GetAllStarredNotesAsync()
        {
            if (!UserClaimUtil.TryGetUserId(User, out Guid userId))
            {
                return Unauthorized("User ID not found.");
            }

            var starredNotes = await starredNotesService.GetAllStarredNotesAsync(userId);
            if (starredNotes is null)
            {
                return NotFound("No Starred Notes Found");
            }

            return Ok(starredNotes);
        }

        [HttpDelete("unmark-as-starrednote/{noteId}"), Authorize(Roles = "DefaultUser")]
        public async Task<IActionResult> UnmarkNotesAsStarredAsync(Guid noteId)
        {
            if (!UserClaimUtil.TryGetUserId(User, out Guid userId))
            {
                return Unauthorized("User ID not found.");
            }

            var unmarkAsStarredNote = await starredNotesService.UnmarkNoteAsStarredAsync(noteId);
            if (!unmarkAsStarredNote)
            {
                return NotFound("Notes Is Not Mark As Favorite");
            }

            return Ok($"Note Unmarked As Starred: {unmarkAsStarredNote}");
        }

        #endregion Public Methods
    }
}