using NTS.Client.Models;
using NTS.Client.Services.Contracts;

namespace NTS.Client.Services
{
    public class FavoriteNotesService : IFavoriteNotesService
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<FavoriteNotesService> logger;

        public FavoriteNotesService(HttpClient httpClient, ILogger<FavoriteNotesService> logger)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public async Task MarkAsFavoriteNoteAsync(FavoriteNotes request, Guid noteId)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"/api/FavoriteNotes/mark-favorite/{noteId}", request);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception error)
            {
                logger.LogError($"Error Marking Note As Favorite: {error.Message}");
            }
        }


        public async Task UnmarkNoteAsFavoriteNoteAsync(Guid noteId)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"/api/FavoriteNotes/unmark-as-favoritenote/{noteId}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception error)
            {
                logger.LogError($"Error Unmarking Note As Favorite: {error.Message}");
            }
        }


        public async Task<List<FavoriteNotes>> GetAllFavoriteNotesAsync()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<List<FavoriteNotes>>("/api/FavoriteNotes/get-all-favoritenotes") ?? new List<FavoriteNotes>();
            }
            catch (Exception error)
            {
                logger.LogError($"Error Fetching All Favorite Notes: {error.Message}");
                return new List<FavoriteNotes>();
            }
        }
    }
}
