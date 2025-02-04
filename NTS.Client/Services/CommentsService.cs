using NTS.Client.Models;
using NTS.Client.Services.Contracts;
using System.Net.Http;

namespace NTS.Client.Services
{
    public class CommentsService : ICommentsService
    {
        private readonly HttpClient httpClient;

        public CommentsService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<Comment>> GetCommentsForNoteAsync(Guid noteId)
        {
            var response = await httpClient.GetAsync($"/api/get-comments/{noteId}");
            response.EnsureSuccessStatusCode();

            var comments = await response.Content.ReadFromJsonAsync<List<Comment>>();
            return comments ?? new List<Comment>();
        }
    }
}
