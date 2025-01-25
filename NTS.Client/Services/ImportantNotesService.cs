using NTS.Client.Models;
using NTS.Client.Services.Contracts;

namespace NTS.Client.Services
{
    public class ImportantNotesService : IImportantNoteService
    {
        private readonly HttpClient httpClient;

        public ImportantNotesService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task MarkNoteAsImportantAsync(ImportantNotes request, Guid noteId)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"/api/ImportantNotes/mark-important/{noteId}", request);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception error)
            {
                Console.WriteLine($"Error Marking As Important :{error.Message}");
            }
        }
    }
}
