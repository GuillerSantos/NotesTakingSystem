using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTS.Server.Services.Contracts;
using System.Security.Claims;

namespace NTS.Server.Controllers
{

    [ApiController]
   [Route("api/[controller]")]
    public class SharedNotesController : ControllerBase
    {
        private readonly ISharedNotesService sharedNotesService;

        public SharedNotesController(ISharedNotesService sharedNotesService)
        {
            this.sharedNotesService = sharedNotesService;
        }

        [HttpPost("mark-shared/{noteId}"), Authorize(Roles = "DefaultUser")]
        public async Task<IActionResult> MarkNoteAsSharedAsync(Guid noteId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                var userId = Guid.Parse(userIdClaim!.Value);

                var markedNote = await sharedNotesService.MarkNoteAsSharedAsync(noteId, userId);

                if (!markedNote)
                    return BadRequest("Note Not Found Or You Are Not Authorized To Mark This Note As Shared");

                return Ok($"Note Marked As Favorite {markedNote}");
            }
            catch (Exception error)
            { 
                return BadRequest(error.Message);
            }
        }
    }
}
