using Blazored.LocalStorage;
using NTS.Client.Domain.DTOs;
using NTS.Client.Domain.Models;
using NTS.Client.Securities;
using NTS.Client.Services.Contracts;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

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

        public async Task<Response> LoginAsync(LoginDto request)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("/api/Auth/login-users", request);
                var token = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    await localStorageService.SetItemAsync("Token", token); // Token stored under 'Token'
                    await authenticationState.GetAuthenticationStateAsync(); // Refresh state

                    return new Response { IsSuccess = true, Message = "User Logged In Successfully" };
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return new Response { IsSuccess = false, Message = error };
                }
            }
            catch (Exception ex)
            {
                return new Response { IsSuccess = false, Message = "An Error Occurred: " + ex.Message };
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