using NTS.Client.Models;
using NTS.Client.Services.Contracts;

namespace NTS.Client.Services
{
    public class SharedNotesService : ISharedNoteService
    {
        private readonly HttpClient httpClient;

        public SharedNotesService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task MarkNoteAsSharedAsync(SharedNotes request, Guid noteId)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"/api/SharedNotes/mark-shared/{noteId}", request);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception error)
            {
                Console.WriteLine($"Error Marking Note As Shared: {error.Message}");
            }
        }
    }
}
