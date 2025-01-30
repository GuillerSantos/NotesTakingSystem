using NTS.Client.Models.DTOs;
using NTS.Client.Services.Contracts;

namespace NTS.Client.Services
{
    public class NotesService : INotesService
    {
        private readonly HttpClient httpClient;

        public NotesService(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task CreateNoteAsync(NoteDto request)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("/api/Notes/create-note", request);
                response.EnsureSuccessStatusCode();
            }
            catch(Exception error)
            {
                Console.WriteLine($"Error Create Note: {error.Message}");
            }
        }


        public async Task<List<NoteDto>> GetAllNotesAsync()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<List<NoteDto>>("/api/Notes/get-all-notes") ?? new List<NoteDto>();
            }
            catch (Exception error)
            {
                Console.WriteLine($"Error Fetching All Notes: {error.Message}");
                return new List<NoteDto>();
            }
        }


        public async Task<List<NoteDto>> SearchNotesAsync(string searchQuery)
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<List<NoteDto>>($"/api/Notes/search-notes?searchQuery={searchQuery}");
                return response ?? new List<NoteDto>();
            }
            catch (Exception error)
            {
                Console.WriteLine($"Error Searching Notes: {error.Message}");
                return new List<NoteDto>();
            }
        }


        public async Task UpdateNoteAsync(NoteDto request, Guid noteId)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"/api/Notes/update-note/{noteId}", request);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception error)
            {
                Console.Error.WriteLine($"Error Updating Note With ID {noteId}: {error.Message}");
            }
        }


        public async Task RemoveNoteAsync(Guid noteId)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"/api/Notes/remove-note/{noteId}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception error)
            {
                Console.WriteLine($"Error Removing Note With This ID {noteId}: {error.Message}");
            }
        }
    }
}
