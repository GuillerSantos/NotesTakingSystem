using Blazored.LocalStorage;
using NTS.Client.Models;
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
        private readonly ILogger<AuthService> logger;
        private ResponseDto responseDto = new ResponseDto();
        private ResponseTokenDto responseToken = new ResponseTokenDto();

        public AuthService(ILogger<AuthService> logger, HttpClient httpClient, ILocalStorageService localStorageService, CustomAuthenticationStateProvider authenticationState)
        {
            this.logger = logger;
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
                return new ResponseDto { IsSuccess = false, ErrorMessage = $"Network error occurred: {error.Message}" };
            }
            catch (Exception error)
            {
                return new ResponseDto { IsSuccess = false, ErrorMessage = $"An unexpected error occurred: {error.Message}" };
            }
        }


        public async Task<ResponseDto> RegisterDefaultUserAsync(RegisterDto request)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("/api/Auth/register-defaultuser", request);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ResponseDto>();
                    return result ?? new ResponseDto { IsSuccess = false, ResponseMessage = "Empty Response Received" };
                }

                return new ResponseDto { IsSuccess = false, ResponseMessage = "Failed To Register, Please Try Again" };
            }
            catch (HttpRequestException error)
            {
                return new ResponseDto { IsSuccess = false, ErrorMessage = $"Network Error: {error.Message}" };
            }
            catch (InvalidOperationException error)
            {
                logger.LogError($"Invalid Operation: {error.Message}");
                return new ResponseDto { IsSuccess = false, ErrorMessage = $"Invalid Operation: {error.Message}" };
            }
            catch (Exception error)
            {
                logger.LogError($"An Error Occured While Handling Registration: {error.Message}");
                return new ResponseDto { IsSuccess = false, ErrorMessage = $"An Unexpected Error Occurred: {error.Message}" };
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
                    return result ?? new ResponseDto { IsSuccess = false, ResponseMessage = "Empty Response Received." };
                }

                return new ResponseDto { IsSuccess = false, ResponseMessage = "Failed To Send Forgot Password Request" };
            }
            catch (HttpRequestException error)
            {
                return new ResponseDto { IsSuccess = false, ErrorMessage = $"Network Error: {error.Message}" };
            }
            catch (Exception error)
            {
                return new ResponseDto { IsSuccess = false, ErrorMessage = $"An Unexpected Error Occurred: { error.Message}" };
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
                    Console.WriteLine($"Failed To Refresh Tokens: {response.ReasonPhrase}");
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
