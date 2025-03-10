using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTS.Server.Services.Contracts;
using NTS.Server.Utilities;

namespace NTS.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SharedNotesController : ControllerBase
    {
        #region Fields

        private readonly ISharedNotesService sharedNotesService;

        #endregion Fields

        #region Public Constructors

        public SharedNotesController(ISharedNotesService sharedNotesService)
        {
            this.sharedNotesService = sharedNotesService ?? throw new ArgumentNullException(nameof(sharedNotesService));
        }

        #endregion Public Constructors

        #region Public Methods

        [HttpPost("mark-shared/{noteId}"), Authorize(Roles = "DefaultUser")]
        public async Task<IActionResult> MarkNoteAsSharedAsync(Guid noteId)
        {
            if (!UserClaimUtil.TryGetUserId(User, out Guid userId))
            {
                return Unauthorized("User ID not found.");
            }

            var markedNote = await sharedNotesService.MarkNoteAsSharedAsync(noteId, userId);

            if (!markedNote)
                return BadRequest("Note Not Found Or You Are Not Authorized To Mark This Note As Shared");

            return Ok($"Note Marked As Favorite {markedNote}");
        }

        [HttpDelete("unmark-sharednote/{noteId}"), Authorize(Roles = "DefaultUser")]
        public async Task<IActionResult> UnmarkNoteAsSharedAsync(Guid noteId)
        {
            if (!UserClaimUtil.TryGetUserId(User, out Guid userId))
            {
                return Unauthorized("User ID not found.");
            }

            await sharedNotesService.UnmarkNoteAsSharedAsync(noteId);
            return Ok("Note Unmarked As Shared");
        }

        [HttpGet("get-all-shared-notes"), Authorize(Roles = "DefaultUser")]
        public async Task<IActionResult> GetAllSharedNotesAsync()
        {
            if (!UserClaimUtil.TryGetUserId(User, out Guid userId))
            {
                return Unauthorized("User ID not found.");
            }

            var sharedNotes = await sharedNotesService.GetAllSharedNotesAsync();

            if (sharedNotes == null || !sharedNotes.Any())
            {
                return NotFound(new { Message = "No Shared Notes Found." });
            }

            return Ok(sharedNotes);
        }

        #endregion Public Methods
    }
}