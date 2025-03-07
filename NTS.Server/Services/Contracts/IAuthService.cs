using NTS.Server.DTOs;
using NTS.Server.Entities;

namespace NTS.Server.Services.Contracts
{
    public interface IAuthService
    {
        #region Public Methods

        Task<IEnumerable<UsersDto>> GetAllUsersAccounts(int page, int pageSize);

        Task<ApplicationUsers?> RegisterUsersAsync(RegisterDto request, bool isAdmin);

        Task<TokenResponseDto?> LoginUsersAsync(LoginDto request);

        Task<bool> LogoutAsync(Guid userId);

        Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request);

        Task<bool> RemoveAccountAsync(Guid userId, Guid noteId);

        #endregion Public Methods
    }
}