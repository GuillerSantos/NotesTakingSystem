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
            var response = await httpClient.PostAsJsonAsync("/api/Notes/create-note", request);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<NoteDto>> GetAllNotesAsync()
        {
            return await httpClient.GetFromJsonAsync<List<NoteDto>>("/api/Notes/get-all-notes")
                ?? throw new InvalidOperationException("Unexpected null response from API.");
        }

        public async Task<List<NoteDto>> SearchNotesAsync(string searchQuery)
        {
            return await httpClient.GetFromJsonAsync<List<NoteDto>>($"/api/Notes/search-notes?searchQuery={searchQuery}")
                ?? throw new InvalidOperationException("Unexpected null response from API.");
        }

        public async Task UpdateNoteAsync(NoteDto request, Guid noteId)
        {
            var response = await httpClient.PutAsJsonAsync($"/api/Notes/update-note/{noteId}", request);
            response.EnsureSuccessStatusCode();
        }

        public async Task RemoveNoteAsync(Guid noteId)
        {
            var response = await httpClient.DeleteAsync($"/api/Notes/remove-note/{noteId}");
            response.EnsureSuccessStatusCode();
        }

        #endregion Public Methods
    }
}