using NTS.Client.Models;
using NTS.Client.Services.Contracts;

namespace NTS.Client.Services
{
    public class ImportantNotesService : IImportantNotesService
    {
        #region Fields

        private readonly HttpClient httpClient;
        private readonly ILogger<ImportantNotesService> logger;

        #endregion Fields

        #region Public Constructors

        public ImportantNotesService(HttpClient httpClient, ILogger<ImportantNotesService> logger)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task MarkNoteAsImportantAsync(ImportantNotes request, Guid noteId)
        {
            var response = await httpClient.PostAsJsonAsync($"/api/ImportantNotes/mark-important/{noteId}", request);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<ImportantNotes>> GetAllImportantNotesAsync()
        {
            return await httpClient.GetFromJsonAsync<List<ImportantNotes>>("api/ImportantNotes/get-all-importantnotes")
                ?? throw new InvalidOperationException("Unexpected null response from API.");
        }

        public async Task UnmarkNoteAsImportantAsync(Guid noteId)
        {
            var response = await httpClient.DeleteAsync($"/api/ImportantNotes/unmark-as-importantnote/{noteId}");
            response.EnsureSuccessStatusCode();
        }

        #endregion Public Methods
    }
}