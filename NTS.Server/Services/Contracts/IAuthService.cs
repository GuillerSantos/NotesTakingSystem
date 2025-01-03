using NTS.Server.Domain.DTOs;
using NTS.Server.Domain.Entities;

namespace NTS.Server.Services.Contracts
{
    public interface IAuthService
    {
        Task<IEnumerable<UsersDto>> GetAllUsersAccounts();

        Task<ApplicationUsers> RegisterUsersAsync(SignUpDto request, string role = "Role");

        Task<string> LoginUsersAsync(LoginDto request);
    }
}
