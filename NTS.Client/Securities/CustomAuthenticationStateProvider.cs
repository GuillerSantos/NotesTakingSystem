﻿using Blazored.LocalStorage;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace YourApp.Client.Securities
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        #region Fields

        private readonly HttpClient httpClient;
        private readonly ILocalStorageService localStorageService;

        #endregion Fields

        #region Public Constructors

        public CustomAuthenticationStateProvider(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.localStorageService = localStorageService ?? throw new ArgumentNullException(nameof(localStorageService));
        }

        #endregion Public Constructors

        #region Public Methods

        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var parts = jwt.Split('.');
            if (parts.Length != 3)
                throw new FormatException("Invalid JWT format");

            var payload = parts[1];
            var jsonBytes = Convert.FromBase64String(AddPadding(payload));
            var claims = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            return claims?.Select(kvp => new Claim(kvp.Key, kvp.Value?.ToString() ?? string.Empty))
                   ?? Enumerable.Empty<Claim>();
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            ClaimsIdentity identity = new ClaimsIdentity();

            // Retrieve token from localStorage
            var token = await localStorageService.GetItemAsync<string>("Token");

            if (!string.IsNullOrEmpty(token))
            {
                identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        public async Task RefreshAuthenticationStateAsync()
        {
            var state = await GetAuthenticationStateAsync();
            NotifyAuthenticationStateChanged(Task.FromResult(state));
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));
        }

        #endregion Public Methods

        #region Private Methods

        private static string AddPadding(string base64)
        {
            return base64.PadRight(base64.Length + (4 - base64.Length % 4) % 4, '=');
        }

        #endregion Private Methods
    }
}