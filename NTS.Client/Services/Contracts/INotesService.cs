using NTS.Client.DTOs;

namespace NTS.Client.Services.Contracts
{
    public interface INotesService
    {
        #region Public Methods

        Task<List<NoteDto>> GetAllNotesAsync();

        Task<List<NoteDto>> SearchNotesAsync(string searchQuery);

        Task CreateNoteAsync(NoteDto request);

        Task UpdateNoteAsync(NoteDto request, Guid noteId);

        Task RemoveNoteAsync(Guid noteId);

        #endregion Public Methods
    }
}