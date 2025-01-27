using NTS.Client.Models;
using NTS.Client.Services.Contracts;

namespace NTS.Client.Services
{
    public class FavoriteNotesService : IFavoriteNotesService
    {
        private readonly HttpClient httpClient;

        public FavoriteNotesService(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task MarkAsFavoriteNoteAsync(FavoriteNotes request, Guid noteId)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"/apiFavoriteNote/mark-favoritenote/{noteId}", request);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception error)
            {
                Console.WriteLine($"Error Marking Note As Favorite: {error.Message}");
            }
        }
    }
}
