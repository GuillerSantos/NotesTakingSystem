using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTS.Server.Services.Contracts;
using System.Security.Claims;

namespace NTS.Server.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ImportantNotesController : ControllerBase
    {
        private readonly IImpotantNotesService impotantNotesService;

        public ImportantNotesController(IImpotantNotesService impotantNotesService)
        {
            this.impotantNotesService = impotantNotesService ?? throw new ArgumentNullException(nameof(impotantNotesService));
        }

        [HttpPost("mark-important/{noteId}"), Authorize(Roles = "DefaultUser")]
        public async Task<IActionResult> MarkNoteAsImpotantAsync(Guid noteId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                var userId = Guid.Parse(userIdClaim!.Value);

                var markedNote = await impotantNotesService.MarkNoteAsImportantAsync(noteId, userId);

                if (!markedNote)
                    return NotFound("Note Not Found Or You Are Not Auhtorized To Mark This Note As Important");

                return Ok($"Note Marked As Favorite: {markedNote}");
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }
    }
}
