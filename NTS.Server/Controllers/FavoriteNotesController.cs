using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTS.Server.Services.Contracts;
using System.Security.Claims;

namespace NTS.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavoriteNotesController : ControllerBase
    {
        private readonly IFavoriteNoteService favoriteNoteService;

        public FavoriteNotesController(IFavoriteNoteService favoriteNoteService)
        {
            this.favoriteNoteService = favoriteNoteService ?? throw new ArgumentNullException(nameof(favoriteNoteService));
        }


        [HttpPost("mark-favorite/{noteId}"), Authorize(Roles = "DefaultUser")]
        public async Task<IActionResult> MarkNoteAsFavoriteAsync(Guid noteId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                var userId = Guid.Parse(userIdClaim!.Value);
                var markedNote = await favoriteNoteService.MarkNotesAsFavoriteAsync(noteId, userId);
                if (!markedNote)
                {
                    return NotFound("Note Not Found Or Your Are Not Authorized To Mark This Note As Favorite");
                }

                return Ok(markedNote);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }


        [HttpGet("get-all-favoritenotes"), Authorize(Roles = "DefaultUser")]
        public async Task<IActionResult> GetAllFavoriteNotesAsync()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                var userId = Guid.Parse(userIdClaim!.Value);
                var favoriteNotes = await favoriteNoteService.GetAllFavoriteNotesAsync(userId);
                if (favoriteNotes == null)
                {
                    return NotFound("No Favorite Notes Found");
                }

                return Ok(favoriteNotes);
            }
            catch (Exception error)
            {
                return BadRequest($"Error Fetching All Favorite Notes: {error.Message}");
            }
        }


        [HttpDelete("unmark-favoritenote/{noteId}"), Authorize(Roles = "DefaultUser")]
        public async Task<IActionResult> UnmarkNoteAsFavoriteAsync(Guid noteId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                var userId = Guid.Parse(userIdClaim!.Value);
                var unmarkAsFavoriteNote = await favoriteNoteService.UnmarkNoteAsFavoriteAsync(noteId);
                if (!unmarkAsFavoriteNote)
                {
                    return NotFound("Note Is Not Mark As Favorite");
                }

                return Ok($"Note Unmarked As Favorite: {unmarkAsFavoriteNote}");
            }
            catch (Exception error)
            {
                return BadRequest($"Error Unmarking Note As Favorite {error.Message}");
            }
        }
    }
}