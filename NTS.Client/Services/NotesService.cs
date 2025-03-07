using NTS.Client.DTOs;
using NTS.Client.Services.Contracts;

namespace NTS.Client.Services
{
    public class NotesService : INotesService
    {
        #region Fields

        private readonly HttpClient httpClient;
        private readonly ILogger<NotesService> logger;

        #endregion Fields

        #region Public Constructors

        public NotesService(HttpClient httpClient, ILogger<NotesService> logger)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task CreateNoteAsync(NoteDto request)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("/api/Notes/create-note", request);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception error)
            {
                logger.LogError($"Error Create Note: {error.Message}");
            }
        }

        public async Task<List<NoteDto>> GetAllNotesAsync()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<List<NoteDto>>("/api/Notes/get-all-notes") ?? new List<NoteDto>();
            }
            catch (Exception error)
            {
                logger.LogError($"Error Fetching All Notes: {error.Message}");
                return new List<NoteDto>();
            }
        }

        public async Task<List<NoteDto>> SearchNotesAsync(string searchQuery)
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<List<NoteDto>>($"/api/Notes/search-notes?searchQuery={searchQuery}");
                return response ?? new List<NoteDto>();
            }
            catch (Exception error)
            {
                logger.LogError($"Error Searching Notes: {error.Message}");
                return new List<NoteDto>();
            }
        }

        public async Task UpdateNoteAsync(NoteDto request, Guid noteId)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"/api/Notes/update-note/{noteId}", request);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception error)
            {
                logger.LogError($"Error Updating Note With ID {noteId}: {error.Message}");
            }
        }

        public async Task RemoveNoteAsync(Guid noteId)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"/api/Notes/remove-note/{noteId}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception error)
            {
                logger.LogError($"Error Removing Note With This ID {noteId}: {error.Message}");
            }
        }

        #endregion Public Methods
    }
}