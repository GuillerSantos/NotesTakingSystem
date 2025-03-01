using NTS.Client.DTOs;

namespace NTS.Client.Services.Contracts
{
    public interface IAuthService
    {
        Task<ResponseDto> LoginAsync(LoginDto request);

        Task<ResponseDto> ForgotPasswordAsync(ForgotPasswordDto request);

        Task<ResponseDto> RegisterDefaultUserAsync(RegisterDto request);
    }
}
