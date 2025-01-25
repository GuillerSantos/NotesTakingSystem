using NTS.Client.Models;
using NTS.Client.Services.Contracts;

namespace NTS.Client.Services
{
    public class FavoriteNoteService : IFavoriteNoteService
    {
        private readonly HttpClient httpClient;

        public FavoriteNoteService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
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
