using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTS.Server.Services.Contracts;
using NTS.Server.Utilities;

namespace NTS.Server.Controllers
{
    [ApiController, Authorize]
    [Route("api/[controller]")]
    public class ImportantNotesController : ControllerBase
    {
        #region Fields

        private readonly IImpotantNotesService impotantNotesService;

        #endregion Fields

        #region Public Constructors

        public ImportantNotesController(IImpotantNotesService impotantNotesService)
        {
            this.impotantNotesService = impotantNotesService ?? throw new ArgumentNullException(nameof(impotantNotesService));
        }

        #endregion Public Constructors

        #region Public Methods

        [HttpPost("mark-important/{noteId}")]
        public async Task<IActionResult> MarkNoteAsImpotantAsync(Guid noteId)
        {
            if (!UserClaimUtil.TryGetUserId(User, out Guid userId))
            {
                return Unauthorized("User ID not found.");
            }

            var markedNote = await impotantNotesService.MarkNoteAsImportantAsync(noteId, userId);

            if (!markedNote)
                return NotFound("Note Not Found Or You Are Not Auhtorized To Mark This Note As Important");

            return Ok($"Note Marked As Favorite: {markedNote}");
        }

        [HttpGet("get-all-importantnotes")]
        public async Task<IActionResult> GetAllImportantNotesAsync()
        {
            if (!UserClaimUtil.TryGetUserId(User, out Guid userId))
            {
                return Unauthorized("User ID not found.");
            }
            var importantNotes = await impotantNotesService.GetAllImportantNotesAsync(userId);

            if (importantNotes == null)
            {
                return NotFound("No Important Notes Found");
            }

            return Ok(importantNotes);
        }

        [HttpDelete("unmark-as-importantnote/{noteId}")]
        public async Task<IActionResult> UnmarkNoteAsImportantAsync(Guid noteId)
        {
            if (!UserClaimUtil.TryGetUserId(User, out Guid userId))
            {
                return Unauthorized("User ID not found.");
            }

            var unmarkAsImportantNote = await impotantNotesService.UnmarkNoteAsImportantAsync(noteId);
            if (!unmarkAsImportantNote)
            {
                return NotFound("Note Is Not Mark As Important");
            }

            return Ok($"Note Unmarked As Important: {unmarkAsImportantNote}");
        }

        #endregion Public Methods
    }
}