using NTS.Client.Models;
using NTS.Client.Services.Contracts;

namespace NTS.Client.Services
{
    public class SharedNotesService : ISharedNotesService
    {
        private readonly HttpClient httpClient;

        public SharedNotesService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<SharedNotes>> GetAllSharedNotesAsync()
        {
            var response = await httpClient.GetAsync("/api/SharedNotes/get-all-sharednotes");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<SharedNotes>>();
        }

        public async Task<SharedNotes> GetSharedNoteByIdAsync(Guid noteId)
        {
            var response = await httpClient.GetAsync($"api/SharedNotes/{noteId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<SharedNotes>();
        }

        public async Task MarkNoteAsSharedAsync(Guid noteId)
        {
            var response = await httpClient.PostAsJsonAsync($"api/SharedNotes/mark-shared/{noteId}", noteId);
            response.EnsureSuccessStatusCode();
        }


        public async Task UpdateSharedNoteAsync(SharedNotes updatedNote)
        {
            var response = await httpClient.PutAsJsonAsync($"api/SharedNotes/{updatedNote.NoteId}", updatedNote);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteSharedNoteAsync(Guid noteId)
        {
            var response = await httpClient.DeleteAsync($"api/notSharedNotes/{noteId}");
            response.EnsureSuccessStatusCode();
        }
    }

}
