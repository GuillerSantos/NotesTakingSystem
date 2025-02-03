﻿using NTS.Client.Models;
using NTS.Client.Services.Contracts;

namespace NTS.Client.Services
{
    public class SharedNotesService : ISharedNotesService
    {
        private readonly HttpClient httpClient;

        public SharedNotesService(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
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

        public async Task<List<SharedNotes>> GetAllSharedNotesAsync()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<List<SharedNotes>>("api/SharedNotes/get-all-sharednotes") ?? new List<SharedNotes>(); ;
            }
            catch (Exception error)
            {
                Console.WriteLine($"Error Fetching All Shared Notes {error.Message}");
                return new List<SharedNotes>();
            }
        }
    }
}
