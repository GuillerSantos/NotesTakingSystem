using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication.BearerToken;
using NTS.Client.Models.DTOs;
using NTS.Client.Services.Contracts;
using System.Security.Claims;
using YourApp.Client.Securities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NTS.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient httpClient;
        private readonly ILocalStorageService localStorageService;
        private readonly CustomAuthenticationStateProvider authenticationState;
        private ResponseDto responseDto = new ResponseDto();
        private ResponseTokenDto responseToken = new ResponseTokenDto();

        public AuthService(HttpClient httpClient, ILocalStorageService localStorageService, CustomAuthenticationStateProvider authenticationState)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.localStorageService = localStorageService ?? throw new ArgumentNullException(nameof(localStorageService));
            this.authenticationState = authenticationState ?? throw new ArgumentNullException(nameof(authenticationState));
        }


        public async Task<ResponseDto> LoginAsync(LoginDto request)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("/api/Auth/login", request);

                if (response.IsSuccessStatusCode)
                {
                    var tokenResponse = await response.Content.ReadFromJsonAsync<ResponseTokenDto>();

                    if (tokenResponse?.AccessToken != null)
                    {
                        await localStorageService.SetItemAsync("Token", tokenResponse.AccessToken);
                        await localStorageService.SetItemAsync("RefreshToken", tokenResponse.RefreshToken);
                        await authenticationState.RefreshAuthenticationStateAsync();

                        responseDto.IsSuccess = true;
                        responseDto.ResponseMessage = "Login Successfully";
                        return responseDto;
                    }
                }

                responseDto.IsSuccess = false;
                responseDto.ErrorMessage = "Invalid Credentials. Please Try Again";
                return responseDto;
            }
            catch (HttpRequestException error)
            {
                throw new Exception($"Network error occurred: {error.Message}");
            }
            catch (Exception error)
            {
                throw new Exception($"An unexpected error occurred: {error.Message}");
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
                    return result ?? new ResponseDto { IsSuccess = false, ResponseMessage = "Empty response received." };
                }

                return new ResponseDto { IsSuccess = false, ResponseMessage = "Failed To Send Forgot Password Request" };
            }
            catch (HttpRequestException ex)
            {
                return new ResponseDto { IsSuccess = false, ResponseMessage = $"Network error: {ex.Message}" };
            }
            catch (Exception error)
            {
                return new ResponseDto { IsSuccess = false, ResponseMessage = "An unexpected error occurred: " + error.Message };
            }
        }


        public async Task<RegisterDto> RegisterDefaultUserAsync(RegisterDto request)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("/api/register-defaultuser", request);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<RegisterDto>();
                }
                else
                {
                    throw new Exception(await response.Content.ReadAsStringAsync());
                }
            }
            catch (HttpRequestException error)
            {
                throw new Exception(error.Message);
            }
        }

        public async Task<bool> RefreshTokenAsync()
        {
            try
            {
                var refreshToken = await localStorageService.GetItemAsync<string>("RefreshToken");

                if (string.IsNullOrEmpty(refreshToken))
                {
                    Console.WriteLine("No Refresh Token Or User Id Found");
                    return false;
                }

                var requestDto = new ResponseTokenDto { ResetToken = responseToken.ResetToken };

                var response = await httpClient.PostAsJsonAsync("api/Auth/refresh-token", responseToken);

                if (response.IsSuccessStatusCode)
                {
                    var tokenResponse = await response.Content.ReadFromJsonAsync<ResponseTokenDto>();

                    if (tokenResponse is not null && !string.IsNullOrEmpty(tokenResponse.AccessToken))
                    {
                        await localStorageService.SetItemAsync("Token", tokenResponse.AccessToken);
                        await authenticationState.RefreshAuthenticationStateAsync();

                        return true;
                    }
                }
                else
                {
                    Console.WriteLine("Failed To Refresh Tokens: " + response.ReasonPhrase);
                }
            }
            catch (Exception error)
            {
                Console.WriteLine($"Error Refreshing Token: {error.Message}");
            }

            return false;
        }
    }
}
