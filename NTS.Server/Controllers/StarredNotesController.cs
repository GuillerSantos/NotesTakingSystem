using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTS.Server.Services.Contracts;
using System.Security.Claims;

namespace NTS.Server.Controllers
{
    [ApiController]
    [Route("api[controller]")]
    public class StarredNotesController : ControllerBase
    {
        private readonly IStarredNotesService starredNotesService;

        public StarredNotesController(IStarredNotesService starredNotesService)
        {
            this.starredNotesService = starredNotesService;
        }

        [HttpPost("mark-shared/{noteId}"), Authorize(Roles = "DefaultUser")]
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
    }
}
