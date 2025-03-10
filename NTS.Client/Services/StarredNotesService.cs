using NTS.Client.Models;
using NTS.Client.Services.Contracts;

namespace NTS.Client.Services
{
    public class StarredNotesService : IStarredNotesService
    {
        #region Fields

        private readonly HttpClient httpClient;
        private readonly ILogger<StarredNotesService> logger;

        #endregion Fields

        #region Public Constructors

        public StarredNotesService(HttpClient httpClient, ILogger<StarredNotesService> logger)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task MarkNoteAsStarredAsync(StarredNotes request, Guid noteId)
        {
            var response = await httpClient.PostAsJsonAsync($"/api/StarredNotes/mark-starred/{noteId}", request);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<StarredNotes>> GetAllStarredNotesAsync()
        {
            return await httpClient.GetFromJsonAsync<List<StarredNotes>>("/api/StarredNotes/get-all-starrednotes") ?? new List<StarredNotes>();
        }

        public async Task UnmarkNoteAsImportantNoteAsync(Guid noteId)
        {
            var response = await httpClient.DeleteAsync($"/api/StarredNotes/unmark-as-starrednote/{noteId}");
            response.EnsureSuccessStatusCode();
        }

        #endregion Public Methods
    }
}