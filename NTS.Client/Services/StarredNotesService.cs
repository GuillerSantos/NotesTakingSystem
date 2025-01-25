using NTS.Client.Models;
using NTS.Client.Services.Contracts;

namespace NTS.Client.Services
{
    public class StarredNotesService : IStarredNotesService
    {
        private readonly HttpClient httpClient;

        public StarredNotesService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task MarkNoteAsStarredAsync(StarredNotes request, Guid noteId)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"/api/StarredNotes/mark-starred/{noteId}", request);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception error)
            {
                Console.WriteLine($"Error Marking Note As Starred: {error.Message}");
            }
        }
    }
}
