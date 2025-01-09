using Blazored.LocalStorage;
using NTS.Client.Domain.DTOs;
using NTS.Client.Domain.Models;
using NTS.Client.Services.Contracts;
using YourApp.Client.Securities;

namespace NTS.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient httpClient;
        private readonly ILocalStorageService localStorageService;
        private readonly CustomAuthenticationStateProvider authenticationState;
        private ResponseToken responseToken = new ResponseToken();

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
                    if (tokenResponse != null)
                    {
                        await localStorageService.SetItemAsync("Token", tokenResponse!.AccessToken);
                        await authenticationState.RefreshAuthenticationStateAsync();
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"An Error Occurred: {ex.Message}");
            }
        }


        public async Task<Response> ForgotPasswordAsync(ForgotPasswordDto request)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("/api/Auth/forgot-password", request);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<Response>().ConfigureAwait(false);
                    return result ?? new Response { IsSuccess = false, Message = "Empty response received." };
                }

                return new Response { IsSuccess = false, Message = "Failed To Send Forgot Password Request" };
            }
            catch (Exception ex)
            {
                return new Response { IsSuccess = false, Message = "An Error Occurred: " + ex.Message };
            }
        }
    }

}