using Microsoft.AspNetCore.Mvc;
using NTS.Server.Domain.DTOs;
using NTS.Server.Domain.Entities;
using NTS.Server.Services.Contracts;

namespace NTS.Server.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly INotesService notesService;

        public NotesController(INotesService notesService)
        {
            this.notesService = notesService;
        }

        [HttpPost("create-note")]
        public async Task<IActionResult> CreateNotesAsync([FromBody] NotesDto request)
        {
            try
            {
                Guid userId = Guid.NewGuid();

                var response = await notesService.CreateNoteAsync(request, userId);

                if (response is null)
                {
                    return StatusCode(500, "Failed To Create Note");
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Creating Note: {ex.Message}");
            }
        }
    }
}
