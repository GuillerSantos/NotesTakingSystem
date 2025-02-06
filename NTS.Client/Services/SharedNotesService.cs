using NTS.Client.Models;
using NTS.Client.Services.Contracts;
using System.Net.Http;

namespace NTS.Client.Services
{
    public class SharedNotesService : ISharedNotesService
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
                HttpResponseMessage response = await httpClient.PostAsJsonAsync($"/api/SharedNotes/mark-shared/{noteId}", request);
                response.EnsureSuccessStatusCode();

            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"HTTP Request Error: {httpEx.Message}");
            }
        }


        public async Task UnmarkNoteAsSharedAsync(Guid noteId)
        {
            try
            {
                HttpResponseMessage response = await httpClient.DeleteAsync($"api/SharedNotes/unmark-sharednote/{noteId}");

                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error unsharing note: {errorMessage}");
                }
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"HTTP Request Error: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
            }
        }


        public async Task<List<SharedNotes>> GetAllSharedNotesAsync(Guid userId)
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<List<SharedNotes>>($"api/SharedNotes/get-all-shared-notes");

                if (response == null)
                {
                    Console.WriteLine("No shared notes found.");
                    return new List<SharedNotes>();
                }

                return response;
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"HTTP Request Error: {httpEx.Message}");
                return new List<SharedNotes>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
                return new List<SharedNotes>();
            }
        }
    }
}