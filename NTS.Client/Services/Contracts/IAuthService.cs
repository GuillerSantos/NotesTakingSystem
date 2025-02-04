using NTS.Client.DTOs;
using NTS.Client.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NTS.Client.Services.Contracts
{
    public interface IAuthService
    {
        Task<ResponseDto> LoginAsync(LoginDto request);

        Task<ResponseDto> ForgotPasswordAsync(ForgotPasswordDto request);

        Task<ResponseDto> RegisterDefaultUserAsync(RegisterDto request);
    }
}
