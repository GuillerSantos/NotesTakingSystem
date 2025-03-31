using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTS.Server.Services.Contracts;
using NTS.Server.Utilities;

namespace NTS.Server.Controllers
{
    [ApiController, Authorize]
    [Route("api/[controller]")]
    public class FavoriteNotesController : ControllerBase
    {
        #region Fields

        private readonly IFavoriteNoteService favoriteNoteService;

        #endregion Fields

        #region Public Constructors

        public FavoriteNotesController(IFavoriteNoteService favoriteNoteService)
        {
            this.favoriteNoteService = favoriteNoteService ?? throw new ArgumentNullException(nameof(favoriteNoteService));
        }

        #endregion Public Constructors

        #region Public Methods

        [HttpPost("mark-favorite/{noteId}")]
        public async Task<IActionResult> MarkNoteAsFavoriteAsync(Guid noteId)
        {
            if (!UserClaimUtil.TryGetUserId(User, out Guid userId))
            {
                return Unauthorized("User ID not found.");
            }

            var markedNote = await favoriteNoteService.MarkNotesAsFavoriteAsync(noteId, userId);
            if (!markedNote)
            {
                return NotFound("Note Not Found Or Your Are Not Authorized To Mark This Note As Favorite");
            }

            return Ok(markedNote);
        }

        [HttpGet("get-all-favoritenotes")]
        public async Task<IActionResult> GetAllFavoriteNotesAsync()
        {
            if (!UserClaimUtil.TryGetUserId(User, out Guid userId))
            {
                return Unauthorized("User ID not found.");
            }

            var favoriteNotes = await favoriteNoteService.GetAllFavoriteNotesAsync(userId);
            if (favoriteNotes == null)
            {
                return NotFound("No Favorite Notes Found");
            }

            return Ok(favoriteNotes);
        }

        [HttpDelete("unmark-as-favoritenote/{noteId}")]
        public async Task<IActionResult> UnmarkNoteAsFavoriteAsync(Guid noteId)
        {
            if (!UserClaimUtil.TryGetUserId(User, out Guid userId))
            {
                return Unauthorized("User ID not found.");
            }

            var unmarkAsFavoriteNote = await favoriteNoteService.UnmarkNoteAsFavoriteAsync(noteId);
            if (!unmarkAsFavoriteNote)
            {
                return NotFound("Note Is Not Mark As Favorite");
            }

            return Ok($"Note Unmarked As Favorite: {unmarkAsFavoriteNote}");
        }

        #endregion Public Methods
    }
}