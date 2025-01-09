using NTS.Server.Domain.DTOs;
using NTS.Server.Domain.Entities;

namespace NTS.Server.Services.Contracts
{
    public interface IAuthService
    {
        Task<IEnumerable<UsersDto>> GetAllUsersAccounts(int page, int pageSize);

        Task<ApplicationUsers?> RegisterUsersAsync(SignUpDto request);

        Task<TokenResponseDto?> LoginUsersAsync(LoginDto request);

        Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request);
    }
}