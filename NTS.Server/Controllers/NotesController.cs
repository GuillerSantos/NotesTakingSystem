using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTS.Server.Entities;
using NTS.Server.Entities.DTOs;
using NTS.Server.Services.Contracts;
using System.Security.Claims;

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

        [HttpPost("create-note"), Authorize (Roles = "DefaultUser")]
        public async Task<IActionResult> CreateNotesAsync([FromBody] NotesDto request)
        {
            try
            {
                var userId = Guid.NewGuid();
                var note = await notesService.CreateNoteAsync(request, userId);

                return CreatedAtAction(nameof(notesService.GetNoteByIdAsync), 
                    new { noteId = note!.NoteId }, note);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Creating Note: {ex.Message}", ex.InnerException);
            }
        }

        [HttpPost("edit-note/{notesId}"), Authorize(Roles = "DefaultUser")]
        public async Task<IActionResult> EditNoteAsync([FromBody] EditNotesDto request, Guid noteId)
        {
            try
            {
                var userId = Guid.NewGuid();
                var updatedNote = await notesService.EditNotesAsync(request, noteId, userId);
                if (updatedNote == null)
                    return NotFound("Note Not Found Or You Are Not Authorized To Edit this Note");

                return Ok(updatedNote);
            }
            catch (Exception ex)
            { 
                return BadRequest($"Error Editing Note: {ex.Message}");
            }
        }

        [HttpDelete("remove-note/{noteId}")]
        public async Task<IActionResult> RemoveNoteAsync(Guid noteId)
        {
            try
            {
                var removed = await notesService.RemoveNoteAsync(noteId);
                if (!removed)
                    return NotFound("Note Not Found");

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error Removing Note: {ex.Message}");
            }
        }


        [HttpGet("get-all-note"), Authorize(Roles = "DefaultUser")]
        public async Task<IActionResult> GetAllNotesAsync()
        {
            try
            {
                var userId = Guid.NewGuid();
                var notes = await notesService.GetAllNotesAsync(userId);
                return Ok(notes);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error Fetching Notes: {ex.Message}");
            }
        }

        [HttpGet("get-note/{noteId}")]
        public async Task<IActionResult> GetNoteByIdAsync(Guid noteId)
        {
            try
            {
                var userId = Guid.NewGuid();
                var note = await notesService.GetNoteByIdAsync(noteId, userId);
                if (note == null)
                    return NotFound("Note Not Found");

                return Ok(note);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error Fetching Note: {ex.Message}");
            }
        }

        [HttpGet("serach-note")]
        public async Task<IActionResult> SearchNotesAsync([FromQuery] string searchTerm)
        {
            try
            {
                var notes = await notesService.SearchNotesAsync(searchTerm);
                return Ok(notes);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error Searching Notes: {ex.Message}");
            }
        }

        [HttpPost("mark-favorite/{noteId}")]
        public async Task<IActionResult> MarkNoteFavoriteAsync(Guid noteId)
        {
            try
            {
                var userId = Guid.NewGuid();
                var success = await notesService.MarkNoteAsFavoriteAsync(noteId, userId);
                if (!success)
                    return NotFound("Note Not Found Or You Are Not Authorized To Mark This Note As Favorite");

                return NotFound("Note Marked as Favorite");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error Marking Note As Favorite: {ex.Message}");
            }
        }

        [HttpPost("mark-important/{noteId}")]
        public async Task<IActionResult> MarkNoteAsImportantAsync(Guid noteId)
        {
            try
            {
                var userId = Guid.NewGuid();
                var success = await notesService.MarkNoteAsImportantAsync(noteId, userId);
                if (!success)
                    return NotFound("Note Not Found Or You Are Not Authorized To Mark This Note As Important");

                return Ok("Note Marked As Important");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error Marking Note As Importand: {ex.Message}");
            }
        }

        [HttpPost("mark-shared/{noteId}")]
        public async Task<IActionResult> MarkNoteAsSharedAsync(Guid noteId, [FromQuery] Guid sharedWithUserId)
        {
            try
            {
                var userId = Guid.NewGuid();
                var success = await notesService.MarkNoteAsSharedAsync(noteId, userId, sharedWithUserId);
                if (!success)
                    return NotFound("Note Not Found Or You Are Not Authorized To Shared This Note");

                return Ok("Note  Shared");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error Sharing Note: {ex.Message}");
            }
        }

        [HttpPost("mark-starred/{noteId}")]
        public async Task<IActionResult> MarkNoteAsStarredAsync(Guid noteId)
        {
            try
            {
                var userId = Guid.NewGuid();
                var success = await notesService.MarkNoteAsStarredAsync(noteId, userId);
                if (!success)
                    return NotFound("Note Not Found Or You Are Not Authorized To Mark This Note As Starred");

                return Ok("Note Marked As Starred");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error Marking Note As Starred: {ex.Message}");
            }
        }
    }
}
