using Blazored.LocalStorage;
using NTS.Client.Models.DTOs;
using NTS.Client.Services.Contracts;
using YourApp.Client.Securities;

namespace NTS.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient httpClient;
        private readonly ILocalStorageService localStorageService;
        private readonly CustomAuthenticationStateProvider authenticationState;

        public AuthService(HttpClient httpClient, ILocalStorageService localStorageService, CustomAuthenticationStateProvider authenticationState)
        {
            this.httpClient = httpClient;
            this.localStorageService = localStorageService;
            this.authenticationState = authenticationState;
        }

        public async Task<bool> LoginAsync(LoginDto request)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("/api/Auth/login", request);

                if (response.IsSuccessStatusCode)
                {
                    var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponseDto>();
                    if (tokenResponse?.AccessToken != null)
                    {
                        await localStorageService.SetItemAsync("Token", tokenResponse.AccessToken);
                        await authenticationState.RefreshAuthenticationStateAsync();
                        return true;
                    }
                }

                return false;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Network error occurred: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<ResponseDto> ForgotPasswordAsync(ForgotPasswordDto request)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("/api/Auth/forgot-password", request);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ResponseDto>();
                    return result ?? new ResponseDto { IsSuccess = false, Message = "Empty response received." };
                }

                return new ResponseDto { IsSuccess = false, Message = "Failed To Send Forgot Password Request" };
            }
            catch (HttpRequestException ex)
            {
                return new ResponseDto { IsSuccess = false, Message = $"Network error: {ex.Message}" };
            }
            catch (Exception ex)
            {
                return new ResponseDto { IsSuccess = false, Message = "An unexpected error occurred: " + ex.Message };
            }
        }
    }
}
