using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTS.Server.Services;
using NTS.Server.Services.Contracts;
using System.Security.Claims;

namespace NTS.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StarredNotesController : ControllerBase
    {
        private readonly IStarredNotesService starredNotesService;

        public StarredNotesController(IStarredNotesService starredNotesService)
        {
            this.starredNotesService = starredNotesService ?? throw new ArgumentNullException(nameof(starredNotesService));
        }

        [HttpPost("mark-starred/{noteId}"), Authorize(Roles = "DefaultUser")]
        public async Task<IActionResult> MarkNoteAsStarredAsync(Guid noteId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                var userid = Guid.Parse(userIdClaim!.Value);
                var markedNote = await starredNotesService.MarkNoteAsStarredAsync(noteId, userid);
                if (!markedNote)
                    return NotFound("Note Not Found Or You Are Not Authorized To Mark This Note As Starred");

                return Ok($"Note Marked Successfully: {markedNote}");
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }


        [HttpGet("get-all-starrednotes"), Authorize(Roles = "DefaultUser")]
        public async Task<IActionResult> GetAllStarredNotesAsync()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                var userId = Guid.Parse(userIdClaim!.Value);
                var starredNotes = await starredNotesService.GetAllStarredNotesAsync(userId);
                if (starredNotes is null)
                {
                    return NotFound("No Starred Notes Found");
                }

                return Ok(starredNotes);
            }
            catch (Exception error)
            {
                return BadRequest($"Error Fetching All Starred Notes: {error.Message}"); 
            }
        }


        [HttpDelete("unmark-as-starrednote/{noteId}"), Authorize(Roles = "DefaultUser")]
        public async Task<IActionResult> UnmarkNotesAsStarredAsync(Guid noteId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                var userId = Guid.Parse(userIdClaim!.Value);
                var unmarkAsStarredNote = await starredNotesService.UnmarkNoteAsStarredAsync(noteId);
                if (!unmarkAsStarredNote)
                {
                    return NotFound("Notes Is Not Mark As Favorite");
                }

                return Ok($"Note Unmarked As Starred: {unmarkAsStarredNote}");
            }
            catch (Exception error)
            {
                return BadRequest($"Error Unmarking As Starred Note :{error.Message}");
            }
        }
    }
}
