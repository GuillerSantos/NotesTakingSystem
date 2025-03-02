using NTS.Client.Models;
using NTS.Client.Services.Contracts;

namespace NTS.Client.Services
{
    public class ImportantNotesService : IImportantNotesService
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<ImportantNotesService> logger;

        public ImportantNotesService(HttpClient httpClient, ILogger<ImportantNotesService> logger)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
                logger.LogError($"Error Marking As Important :{error.Message}");
            }
        }


        public async Task<List<ImportantNotes>> GetAllImportantNotesAsync()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<List<ImportantNotes>>("api/ImportantNotes/get-all-importantnotes") ?? new List<ImportantNotes>();
            }
            catch (Exception error)
            {
                logger.LogError($"Error Fetching All Important Notes: {error.Message}");
                return new List<ImportantNotes>();
            }
        }


        public async Task UnamrkNoteAsImportantAsync(Guid noteId)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"/api/ImportantNotes/unmark-as-importantnote/{noteId}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception error)
            {
                logger.LogError($"Error Unmaking Note :{error.Message}");
            }
        }
    }
}
