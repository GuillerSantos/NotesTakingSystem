using NTS.Server.Entities;
using NTS.Server.Entities.DTOs;

namespace NTS.Server.Services.Contracts
{
    public interface IAuthService
    {
        Task<IEnumerable<UsersDto>> GetAllUsersAccounts(int page, int pageSize);

        Task<ApplicationUsers?> RegisterUsersAsync(SignUpDto request, bool isAdmin);

        Task<TokenResponseDto?> LoginUsersAsync(LoginDto request);

        Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request);
    }
}