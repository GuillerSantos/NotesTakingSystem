using NTS.Client.Models;
using NTS.Client.Services.Contracts;

namespace NTS.Client.Services
{
    public class StarredNotesService : IStarredNotesService
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<StarredNotesService> logger;

        public StarredNotesService(HttpClient httpClient, ILogger<StarredNotesService> logger)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.logger = logger;
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
                logger.LogError($"Error Marking Note As Starred: {error.Message}");
            }
        }


        public async Task<List<StarredNotes>> GetAllStarredNotesAsync()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<List<StarredNotes>>("/api/StarredNotes/get-all-starrednotes") ?? new List<StarredNotes>();
            }
            catch (Exception error)
            {
                logger.LogError($"Error Fetching All Starred Notes: {error.Message}");
                return new List<StarredNotes>();
            }
        }


        public async Task UnmarkNoteAsImportantNoteAsync(Guid noteId)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"/api/StarredNotes/unmark-as-starrednote/{noteId}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception error)
            {
                logger.LogError($"Error Unmarking Notes: {error.Message}");
            }
        }
    }
}
