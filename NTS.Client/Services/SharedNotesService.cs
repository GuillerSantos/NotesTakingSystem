using NTS.Client.Models;
using NTS.Client.Services.Contracts;

namespace NTS.Client.Services
{
    public class SharedNotesService : ISharedNotesService
    {
        #region Fields

        private readonly HttpClient httpClient;
        private readonly ILogger<SharedNotesService> logger;

        #endregion Fields

        #region Public Constructors

        public SharedNotesService(HttpClient httpClient, ILogger<SharedNotesService> logger)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task MarkNoteAsSharedAsync(SharedNotes request, Guid noteId)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync($"/api/SharedNotes/mark-shared/{noteId}", request);
            response.EnsureSuccessStatusCode();
        }

        public async Task UnmarkNoteAsSharedAsync(Guid noteId)
        {
            HttpResponseMessage response = await httpClient.DeleteAsync($"api/SharedNotes/unmark-sharednote/{noteId}");

            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                logger.LogError($"Error unsharing note: {errorMessage}");
            }
        }

        public async Task<List<SharedNotes>> GetAllSharedNotesAsync()
        {
            var response = await httpClient.GetFromJsonAsync<List<SharedNotes>>($"api/SharedNotes/get-all-shared-notes");

            if (response == null)
            {
                logger.LogError("No shared notes found.");
                return new List<SharedNotes>();
            }

            return response;
        }

        #endregion Public Methods
    }
}