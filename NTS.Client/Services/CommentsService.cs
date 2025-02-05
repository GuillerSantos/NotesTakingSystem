﻿using NTS.Client.Models;
using NTS.Client.Services.Contracts;
using System.Net.Http;
using System.Net.Http.Json;

namespace NTS.Client.Services
{
    public class CommentsService : ICommentsService
    {
        private readonly HttpClient httpClient;

        public CommentsService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public event Action<Guid, Guid, string, string, string, DateTime> OnCommentReceived;

        public async Task<List<Comment>> GetCommentsForNoteAsync(Guid noteId)
        {
            var response = await httpClient.GetAsync($"/api/get-comments/{noteId}");
            response.EnsureSuccessStatusCode();

            var comments = await response.Content.ReadFromJsonAsync<List<Comment>>();
            return comments ?? new List<Comment>();
        }

        public async Task SendCommentAsync(Guid noteId, Guid userId, string title, string fullName, DateTime createdAt, string content)
        {
            var comment = new Comment
            {
                NoteId = noteId,
                UserId = userId,
                Title = title,
                FullName = fullName,
                CreatedAt = createdAt,
                Content = content
            };

            var response = await httpClient.PostAsJsonAsync("/api/send-comment", comment);
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                OnCommentReceived?.Invoke(noteId, userId, title, fullName, content, createdAt);
            }
        }

        public Task StartAsync()
        {
            throw new NotImplementedException("StartAsync logic is not yet implemented.");
        }
    }
}
