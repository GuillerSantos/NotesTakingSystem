using NTS.Client.Models.DTOs;

namespace NTS.Client.Services.Contracts
{
    public interface IAuthService
    {
        Task<ResponseDto> LoginAsync(LoginDto request);

        Task LogoutAsync();

        Task<ResponseDto> ForgotPasswordAsync(ForgotPasswordDto request);

        Task<RegisterDto> RegisterDefaultUserAsync(RegisterDto request);
    }
}
