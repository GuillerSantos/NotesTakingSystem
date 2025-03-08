﻿using NTS.Client.Models;
using NTS.Client.Services.Contracts;

namespace NTS.Client.Services
{
    public class SharedNotesService : ISharedNotesService
    {
        #region Fields

        private readonly HttpClient httpClient;
        private readonly ILogger<SharedNotesService> logger;

        #endregion Fields

        #region Public Constructors

        public SharedNotesService(HttpClient httpClient, ILogger<SharedNotesService> logger)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task MarkNoteAsSharedAsync(SharedNotes request, Guid noteId)
        {
            try
            {
                HttpResponseMessage response = await httpClient.PostAsJsonAsync($"/api/SharedNotes/mark-shared/{noteId}", request);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException httpEx)
            {
                logger.LogError($"HTTP Request Error: {httpEx.Message}");
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
                    logger.LogError($"Error unsharing note: {errorMessage}");
                }
            }
            catch (HttpRequestException httpEx)
            {
                logger.LogError($"HTTP Request Error: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                logger.LogError($"Unexpected Error: {ex.Message}");
            }
        }

        public async Task<List<SharedNotes>> GetAllSharedNotesAsync()
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<List<SharedNotes>>($"api/SharedNotes/get-all-shared-notes");

                if (response == null)
                {
                    logger.LogError("No shared notes found.");
                    return new List<SharedNotes>();
                }

                return response;
            }
            catch (HttpRequestException httpEx)
            {
                logger.LogError($"HTTP Request Error: {httpEx.Message}");
                return new List<SharedNotes>();
            }
            catch (Exception ex)
            {
                logger.LogError($"Unexpected Error: {ex.Message}");
                return new List<SharedNotes>();
            }
        }

        #endregion Public Methods
    }
}