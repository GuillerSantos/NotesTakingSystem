using NTS.Server.Entities;
using NTS.Server.Entities.DTOs;

namespace NTS.Server.Services.Contracts
{
    public interface IAuthService
    {
        Task<IEnumerable<UsersDto>> GetAllUsersAccounts(int page, int pageSize);

        Task<ApplicationUsers?> RegisterUsersAsync(RegisterDto request, bool isAdmin);

        Task<TokenResponseDto?> LoginUsersAsync(LoginDto request);

        Task<bool> LogoutAsync(Guid userId);

        Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request);

        Task<bool> RemoveAccountAsync(Guid userId, Guid noteId);
    }
}