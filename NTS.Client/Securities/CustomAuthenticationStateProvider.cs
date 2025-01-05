using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Net.Http.Headers;
using System.Text.Json;

namespace NTS.Client.Securities
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient httpClient;
        private readonly ILocalStorageService localStorageService;
        private string? token;

        public CustomAuthenticationStateProvider(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            this.httpClient = httpClient;
            this.localStorageService = localStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();

            try
            {
                token = await localStorageService.GetItemAsStringAsync("token");

                if (!string.IsNullOrEmpty(token))
                {
                    identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
                    httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error During Authentication State Retrieval: {ex.Message}");
            }

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }


        public async Task RefreshAuthenticationStateAsync()
        {
            try
            {
                var state = await GetAuthenticationStateAsync();
                NotifyAuthenticationStateChanged(Task.FromResult(state));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Refreshing Authentication State: {ex.Message}");
            }
        }

        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            try
            {
                var parts = jwt.Split('.');
                if (parts.Length != 3) throw new FormatException("Invalid JWT Format");

                var payload = parts[1];
                var jsonBytes = Convert.FromBase64String(AddPadding(payload));
                var claims = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

                return claims?.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!)) ?? Enumerable.Empty<Claim>();
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Invalid Token Format: {ex.Message}");
                return Enumerable.Empty<Claim>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing claims from JWT: {ex.Message}");
                return Enumerable.Empty<Claim>();
            }
        }

        private static string AddPadding(string base64)
        {
            return base64.PadRight(base64.Length + (4 - base64.Length % 4) % 4, '=');
        }
    }
}
