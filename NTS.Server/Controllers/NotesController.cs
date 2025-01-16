using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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


        [HttpPost("create-note"), Authorize(Roles = "DefaultUser")]
        public async Task<IActionResult> CreateNoteAsync([FromBody] CreateNotesDto request)
        {
            try
            {
                // Retrieves UserId From The Claims(Authenticated User)
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim == null)
                {
                    return Unauthorized("User Id Not Found In Token");
                }

                // Convert UserId Claim Value To A Guid
                var userId = Guid.Parse(userIdClaim.Value);

                var note = await notesService.CreateNoteAsync(request, userId);

                if (note == null)
                {
                    return BadRequest("Failed To Create The Note");
                }

                return CreatedAtAction("GetNoteById", new { noteId = note.NoteId }, note);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error Creating Note: {ex.Message}");
            }
        }


        [HttpPost("edit-note/{noteId}"), Authorize(Roles = "DefaultUser")]
        public async Task<IActionResult> EditNoteAsync([FromBody] EditNotesDto request, Guid noteId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim == null)
                {
                    return Unauthorized("User Id Not Found In Token");
                }

                var userId = Guid.Parse(userIdClaim.Value);

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


        [HttpDelete("remove-note/{noteId}"), Authorize (Roles = "DefaultUser")]
        public async Task<IActionResult> RemoveNoteAsync(Guid noteId)
        {
            try
            {
                var removed = await notesService.RemoveNoteAsync(noteId);

                if (!removed)
                {
                    return NotFound("Note Not Found");
                }
                else
                {
                    return Ok("Successfully Removed Notes");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error Removing Note: {ex.Message}");
            }
        }


        [HttpGet("get-all-notes"), Authorize(Roles = "DefaultUser")]
        public async Task<IActionResult> GetAllNotesAsync()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim == null)
                {
                    return Unauthorized("User Id Not Found In Token");
                }

                var userId = Guid.Parse(userIdClaim.Value);

                var notes = await notesService.GetAllNotesAsync(userId);

                if (notes == null)
                {
                    return NotFound("No Notes Found");
                }

                return Ok(notes);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error Fetching Notes: {ex.Message}");
            }
        }


        [HttpGet("get-note/{noteId}"), Authorize (Roles = "DefaultUser")]
        [ActionName("GetNoteById")]
        public async Task<IActionResult> GetNoteByIdAsync(Guid noteId)
        {
            try
            {
                var note = await notesService.GetNoteByIdAsync(noteId);

                if (note == null)
                {
                    return NotFound("No Note Found With the Note Id");
                }

                return Ok(note);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error Fetching Note: {ex.Message}");
            }
        }


        [HttpGet("search-note"), Authorize (Roles = "DefaultUser")]
        public async Task<IActionResult> SearchNotesAsync([FromQuery] string searchTerm)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim == null)
                {
                    return Unauthorized("User Id Not Found In Token");
                }

                var userId = Guid.Parse(userIdClaim.Value);

                var response = await notesService.SearchNotesAsync(searchTerm, userId);

                if (response == null || response.Count() == 0)
                {
                    return NotFound("No Notes Matching Your Search Criteria");
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error Searching Notes: {ex.Message}");
            }
        }


        [HttpPost("mark-favorite/{noteId}"), Authorize (Roles = "DefaultUser")]
        public async Task<IActionResult> MarkNoteFavoriteAsync(Guid noteId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                var userId = Guid.Parse(userIdClaim!.Value);

                var response = await notesService.MarkNoteAsFavoriteAsync(noteId, userId);

                if (!response)
                    return NotFound("Note Not Found Or You Are Not Authorized To Mark This Note As Favorite");

                return Ok($"Note Marked as Favorite: {response}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error Marking Note As Favorite: {ex.Message}");
            }
        }


        [HttpPost("mark-important/{noteId}"), Authorize (Roles = "DefaultUser")]
        public async Task<IActionResult> MarkNoteAsImportantAsync(Guid noteId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                var userId = Guid.Parse(userIdClaim!.Value);

                var response = await notesService.MarkNoteAsImportantAsync(noteId, userId);
               
                if (!response)
                    return NotFound("Note Not Found Or You Are Not Authorized To Mark This Note As Important");

                return Ok($"Note Marked As Important: {response}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error Marking Note As Importand: {ex.Message}");
            }
        }


        [HttpPost("mark-shared/{noteId}"), Authorize (Roles = "DefaultUser")]
        public async Task<IActionResult> MarkNoteAsSharedAsync(Guid noteId, [FromQuery] Guid sharedWithUserId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                var userId = Guid.Parse(userIdClaim!.Value);

                var response = await notesService.MarkNoteAsSharedAsync(noteId, userId, sharedWithUserId);
                
                if (!response)
                    return NotFound("Note Not Found Or You Are Not Authorized To Shared This Note");

                return Ok($"Note  Shared: {response}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error Sharing Note: {ex.Message}");
            }
        }


        [HttpPost("mark-starred/{noteId}"), Authorize (Roles = "DefaultUser")]
        public async Task<IActionResult> MarkNoteAsStarredAsync(Guid noteId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                var userId = Guid.Parse(userIdClaim!.Value);

                var response = await notesService.MarkNoteAsStarredAsync(noteId, userId);

                if (!response)
                    return NotFound("Note Not Found Or You Are Not Authorized To Mark This Note As Starred");

                return Ok($"Note Marked As Starred: {response}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error Marking Note As Starred: {ex.Message}");
            }
        }
    }
}