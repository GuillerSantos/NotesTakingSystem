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
            this.logger = logger;
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


        public async Task UnmarkNoteAsFavoriteNoteAsync(Guid noteId, Guid userId)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"/api/FavoriteNotes/unmark-favoritenote/{noteId}/{userId}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception error)
            {
                logger.LogError($"Error Unmarking Note As Favorite: {error.Message}");
            }
        }


        public async Task GetAllFavoriteNotesAsync()
        {
            
        }
    }
}
