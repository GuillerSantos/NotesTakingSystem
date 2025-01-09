using Blazored.LocalStorage;
using System.Security.Claims;
using System.Net.Http.Headers;
using System.Text.Json;

namespace YourApp.Client.Securities
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient httpClient;
        private readonly ILocalStorageService localStorageService;
        private string? token;
        private string? role;

        public CustomAuthenticationStateProvider(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            this.httpClient = httpClient;
            this.localStorageService = localStorageService;
        }

        // Get authentication state asynchronously
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();

            try
            {
                // Retrieve the token and role from localStorage
                token = await localStorageService.GetItemAsStringAsync("Token");

                if (!string.IsNullOrEmpty(token))
                {
                    identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
                    httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during authentication state retrieval: {ex.Message}");
            }

            var claimsPrincipal = new ClaimsPrincipal(identity);

            return new AuthenticationState(claimsPrincipal);
        }

        // Parse JWT to extract claims
        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            try
            {
                var parts = jwt.Split('.');
                if (parts.Length != 3) throw new FormatException("Invalid JWT format");

                var payload = parts[1];
                var jsonBytes = Convert.FromBase64String(AddPadding(payload));
                var claims = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

                return claims?.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!)) ?? Enumerable.Empty<Claim>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing claims from JWT: {ex.Message}");
                return Enumerable.Empty<Claim>();
            }
        }

        // Add padding for Base64 string
        private static string AddPadding(string base64)
        {
            return base64.PadRight(base64.Length + (4 - base64.Length % 4) % 4, '=');
        }

        // Refresh the authentication state
        public async Task RefreshAuthenticationStateAsync()
        {
            try
            {
                var state = await GetAuthenticationStateAsync();
                NotifyAuthenticationStateChanged(Task.FromResult(state));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error refreshing authentication state: {ex.Message}");
            }
        }
    }
}
