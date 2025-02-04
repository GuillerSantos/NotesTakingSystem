using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using NTS.Server.DTOs;
using NTS.Server.Services.Contracts;
using System.Data.Common;
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
            this.notesService = notesService ?? throw new ArgumentNullException(nameof(notesService));
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


        [HttpGet("search-notes"), Authorize(Roles = "DefaultUser")]
        public async Task<IActionResult> SearchNotesAsync([FromQuery] string searchQuery)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim == null)
                {
                    return Unauthorized("User Id Not Found In Token");
                }

                var userId = Guid.Parse(userIdClaim.Value);

                var notes = string.IsNullOrWhiteSpace(searchQuery)
                    ? await notesService.GetAllNotesAsync(userId)
                    : await notesService.SearchNotesAsync(searchQuery, userId);

                return Ok(notes);
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


        [HttpPost("update-note/{noteId}"), Authorize(Roles = "DefaultUser")]
        public async Task<IActionResult> UpdateNoteAsync([FromBody] UpdateNotesDto request, Guid noteId)
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
        public async Task<IActionResult> RemoveNoteAsync(Guid noteId, Guid userId)
        {
            try
            {
                var removedNote = await notesService.RemoveNoteAsync(noteId, userId);

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
    }
}