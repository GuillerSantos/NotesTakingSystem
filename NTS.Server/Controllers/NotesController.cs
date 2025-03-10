using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTS.Server.DTOs;
using NTS.Server.Services.Contracts;
using NTS.Server.Utilities;

namespace NTS.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        #region Fields

        private readonly INotesService notesService;

        #endregion Fields

        #region Public Constructors

        public NotesController(INotesService notesService)
        {
            this.notesService = notesService ?? throw new ArgumentNullException(nameof(notesService));
        }

        #endregion Public Constructors

        #region Public Methods

        [HttpGet("get-all-notes"), Authorize(Roles = "DefaultUser")]
        public async Task<IActionResult> GetAllNotesAsync()
        {
            if (!UserClaimUtil.TryGetUserId(User, out Guid userId))
            {
                return Unauthorized("User ID not found.");
            }

            var fetchedNotes = await notesService.GetAllNotesAsync(userId);

            if (fetchedNotes == null)
            {
                return NotFound("No Notes Found");
            }

            return Ok(fetchedNotes);
        }

        [HttpGet("get-note/{noteId}"), Authorize(Roles = "DefaultUser")]
        [ActionName("GetNoteById")]
        public async Task<IActionResult> GetNoteByIdAsync(Guid noteId)
        {
            var fetchedNote = await notesService.GetNoteByIdAsync(noteId);

            if (fetchedNote == null)
            {
                return NotFound("No Note Found With the Note Id");
            }

            return Ok(fetchedNote);
        }

        [HttpGet("search-notes"), Authorize(Roles = "DefaultUser")]
        public async Task<IActionResult> SearchNotesAsync([FromQuery] string searchQuery)
        {
            if (!UserClaimUtil.TryGetUserId(User, out Guid userId))
            {
                return Unauthorized("User ID not found.");
            }

            var notes = string.IsNullOrWhiteSpace(searchQuery)
                    ? await notesService.GetAllNotesAsync(userId)
                    : await notesService.SearchNotesAsync(searchQuery, userId);

            return Ok(notes);
        }

        [HttpPost("create-note"), Authorize(Roles = "DefaultUser")]
        public async Task<IActionResult> CreateNoteAsync([FromBody] CreateNotesDto request)
        {
            if (!UserClaimUtil.TryGetUserId(User, out Guid userId))
            {
                return Unauthorized("User ID not found.");
            }

            var newNote = await notesService.CreateNoteAsync(request, userId);

            if (newNote == null)
            {
                return BadRequest("Failed To Create The Note");
            }

            return CreatedAtAction("GetNoteById", new { noteId = newNote.NoteId }, newNote);
        }

        [HttpPut("update-note/{noteId}"), Authorize(Roles = "DefaultUser")]
        public async Task<IActionResult> UpdateNoteAsync([FromBody] UpdateNotesDto request, Guid noteId)
        {
            if (!UserClaimUtil.TryGetUserId(User, out Guid userId))
            {
                return Unauthorized("User ID not found.");
            }

            var editedNote = await notesService.UpdateNotesAsync(request, noteId, userId);

            if (editedNote == null)
                return NotFound("Note Not Found Or You Are Not Authorized To Edit this Note");

            return Ok(editedNote);
        }

        [HttpDelete("remove-note/{noteId}"), Authorize(Roles = "DefaultUser")]
        public async Task<IActionResult> RemoveNoteAsync(Guid noteId)
        {
            if (!UserClaimUtil.TryGetUserId(User, out Guid userId))
            {
                return Unauthorized("User ID not found.");
            }

            var removedNote = await notesService.RemoveNoteAsync(noteId, userId);

            if (!removedNote)
            {
                return NotFound("Note Not Found");
            }
            return Ok("Successfully Removed Note");
        }

        #endregion Public Methods
    }
}