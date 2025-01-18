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

                var fetchedNotes = await notesService.GetAllNotesAsync(userId);

                if (fetchedNotes == null)
                {
                    return NotFound("No Notes Found");
                }

                return Ok(fetchedNotes);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }


        [HttpGet("get-note/{noteId}"), Authorize(Roles = "DefaultUser")]
        [ActionName("GetNoteById")]
        public async Task<IActionResult> GetNoteByIdAsync(Guid noteId)
        {
            try
            {
                var fetchedNote = await notesService.GetNoteByIdAsync(noteId);

                if (fetchedNote == null)
                {
                    return NotFound("No Note Found With the Note Id");
                }

                return Ok(fetchedNote);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }


        [HttpGet("search-note"), Authorize(Roles = "DefaultUser")]
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

                var searchedNote = await notesService.SearchNotesAsync(searchTerm, userId);

                if (searchedNote == null || searchedNote.Count() == 0)
                {
                    return NotFound("No Notes Matching Your Search Criteria");
                }

                return Ok(searchedNote);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
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

                var newNote = await notesService.CreateNoteAsync(request, userId);

                if (newNote == null)
                {
                    return BadRequest("Failed To Create The Note");
                }

                return CreatedAtAction("GetNoteById", new { noteId = newNote.NoteId }, newNote);
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }


        [HttpPost("edit-note/{noteId}"), Authorize(Roles = "DefaultUser")]
        public async Task<IActionResult> EditNoteAsync([FromBody] UpdateNotesDto request, Guid noteId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim == null)
                {
                    return Unauthorized("User Id Not Found In Token");
                }

                var userId = Guid.Parse(userIdClaim.Value);

                var editedNote = await notesService.UpdateNotesAsync(request, noteId, userId);

                if (editedNote == null)
                    return NotFound("Note Not Found Or You Are Not Authorized To Edit this Note");

                return Ok(editedNote);
            }
            catch (Exception error)
            { 
                return BadRequest(error.Message);
            }
        }


        [HttpDelete("remove-note/{noteId}"), Authorize (Roles = "DefaultUser")]
        public async Task<IActionResult> RemoveNoteAsync(Guid noteId)
        {
            try
            {
                var removedNote = await notesService.RemoveNoteAsync(noteId);

                if (!removedNote)
                {
                    return NotFound("Note Not Found");
                }
                else
                {
                    return Ok("Successfully Removed Notes");
                }
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }


        [HttpPost("mark-favorite/{noteId}"), Authorize (Roles = "DefaultUser")]
        public async Task<IActionResult> MarkNoteFavoriteAsync(Guid noteId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                var userId = Guid.Parse(userIdClaim!.Value);

                var markedNote = await notesService.MarkNoteAsFavoriteAsync(noteId, userId);

                if (!markedNote)
                    return NotFound("Note Not Found Or You Are Not Authorized To Mark This Note As Favorite");

                return Ok($"Note Marked as Favorite: {markedNote}");
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }


        [HttpPost("mark-important/{noteId}"), Authorize (Roles = "DefaultUser")]
        public async Task<IActionResult> MarkNoteAsImportantAsync(Guid noteId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                var userId = Guid.Parse(userIdClaim!.Value);

                var markedNote = await notesService.MarkNoteAsImportantAsync(noteId, userId);
               
                if (!markedNote)
                    return NotFound("Note Not Found Or You Are Not Authorized To Mark This Note As Important");

                return Ok($"Note Marked As Important: {markedNote}");
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }


        [HttpPost("mark-shared/{noteId}"), Authorize (Roles = "DefaultUser")]
        public async Task<IActionResult> MarkNoteAsSharedAsync(Guid noteId, [FromQuery] Guid sharedWithUserId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                var userId = Guid.Parse(userIdClaim!.Value);

                var markedNote = await notesService.MarkNoteAsSharedAsync(noteId, userId, sharedWithUserId);
                
                if (!markedNote)
                    return NotFound("Note Not Found Or You Are Not Authorized To Shared This Note");

                return Ok($"Note  Shared: {markedNote}");
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }


        [HttpPost("mark-starred/{noteId}"), Authorize (Roles = "DefaultUser")]
        public async Task<IActionResult> MarkNoteAsStarredAsync(Guid noteId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                var userId = Guid.Parse(userIdClaim!.Value);

                var markedNote = await notesService.MarkNoteAsStarredAsync(noteId, userId);

                if (!markedNote)
                    return NotFound("Note Not Found Or You Are Not Authorized To Mark This Note As Starred");

                return Ok($"Note Marked As Starred: {markedNote}");
            }
            catch (Exception error)
            {
                return BadRequest(error.Message);
            }
        }
    }
}