using Blazored.LocalStorage;
using NTS.Client.Domain.DTOs;
using NTS.Client.Domain.Models;
using NTS.Client.Services.Contracts;
using System.Text;
using System.Text.Json;

namespace NTS.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient httpClient;
        private readonly ILocalStorageService localStorageService;
        private readonly AuthenticationStateProvider authenticationState;

        public AuthService(HttpClient httpClient, ILocalStorageService localStorageService, AuthenticationStateProvider authenticationState)
        {
            this.httpClient = httpClient;
            this.localStorageService = localStorageService;
            this.authenticationState = authenticationState;

        }
        public async Task<Response> LoginAsync(LoginDtos request)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("/api/Auth/login-users", request);
                var token = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    await localStorageService.SetItemAsync("Token", token);
                    await authenticationState.GetAuthenticationStateAsync();
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
    }
}
