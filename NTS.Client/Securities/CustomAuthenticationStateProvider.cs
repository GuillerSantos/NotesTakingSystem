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

        public CustomAuthenticationStateProvider(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            this.httpClient = httpClient;
            this.localStorageService = localStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            ClaimsIdentity identity = new ClaimsIdentity();

            try
            {
                // Retrieve token from localStorage
                var token = await localStorageService.GetItemAsync<string>("Token");

                if (!string.IsNullOrEmpty(token))
                {
                    identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
                    httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                }
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"JSON parsing error during authentication state: {jsonEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving authentication state: {ex.Message}");
            }

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            try
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
            catch (FormatException formatEx)
            {
                Console.WriteLine($"Invalid JWT format: {formatEx.Message}");
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

        public async Task LogoutAsync()
        {
            await localStorageService.RemoveItemAsync("Token");
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));
        }
    }
}
