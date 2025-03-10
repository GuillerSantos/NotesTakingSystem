using NTS.Client.Models;
using NTS.Client.Services.Contracts;

namespace NTS.Client.Services
{
    public class FavoriteNotesService : IFavoriteNotesService
    {
        #region Fields

        private readonly HttpClient httpClient;
        private readonly ILogger<FavoriteNotesService> logger;

        #endregion Fields

        #region Public Constructors

        public FavoriteNotesService(HttpClient httpClient, ILogger<FavoriteNotesService> logger)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task MarkAsFavoriteNoteAsync(FavoriteNotes request, Guid noteId)
        {
            var response = await httpClient.PostAsJsonAsync($"/api/FavoriteNotes/mark-favorite/{noteId}", request);
            response.EnsureSuccessStatusCode();
        }

        public async Task UnmarkNoteAsFavoriteNoteAsync(Guid noteId)
        {
            var response = await httpClient.DeleteAsync($"/api/FavoriteNotes/unmark-as-favoritenote/{noteId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<FavoriteNotes>> GetAllFavoriteNotesAsync()
        {
            return await httpClient.GetFromJsonAsync<List<FavoriteNotes>>("/api/FavoriteNotes/get-all-favoritenotes")
                ?? throw new InvalidOperationException("Unexpected null response from API.");
        }

        #endregion Public Methods
    }
}