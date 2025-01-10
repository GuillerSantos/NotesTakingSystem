using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NTS.Server.Database.DatabaseContext;
using NTS.Server.Domain.DTOs;
using NTS.Server.Domain.Entities;
using NTS.Server.Services.Contracts;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace NTS.Server.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IConfiguration configuration;

        public AuthService(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
        }


        public async Task<TokenResponseDto?> LoginUsersAsync(LoginDto request)
        {
            try
            {
                var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
                if (user is null)
                {
                    return null;
                }

                if (new PasswordHasher<ApplicationUsers>().VerifyHashedPassword(user, user.PasswordHash, request.Password)
                    == PasswordVerificationResult.Failed)
                {
                    return null;
                }

                return await CreateTokenResponse(user);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Logging In User: {ex.Message}", ex);
            }
        }


        private async Task<TokenResponseDto> CreateTokenResponse(ApplicationUsers? user)
        {
            return new TokenResponseDto()
            {
                AccessToken = CreateToken(user!),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user!)
            };
        }


        public async Task<ApplicationUsers?> RegisterUsersAsync(SignUpDto request)
        {
            try
            {
                if (await dbContext.Users.AnyAsync(u => u.Email == request.Email))
                {
                    return null;
                }

                var user = new ApplicationUsers();
                var hashPassword = new PasswordHasher<ApplicationUsers>()
                    .HashPassword(user, request.Password);

                user.FullName = request.FullName;
                user.Email = request.Email;
                user.PasswordHash = hashPassword;
                user.Role = request.Role;
                user.PhoneNumber = request.PhoneNumber;
                user.RecoveryEmail = request.RecoveryEmail;
                user.DateJoined = DateTime.UtcNow;

                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Registering User: {ex.Message}", ex);
            }
        }


        public async Task<IEnumerable<UsersDto>> GetAllUsersAccounts(int page, int pageSize)
        {
            try
            {
                return await dbContext.Users
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(u => new UsersDto
                    {
                        UserId = u.UserId,
                        FullName = u.FullName,
                        Email = u.Email,
                        Role = u.Role,
                        DateJoined = u.DateJoined,
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Retrieving All Users: {ex.Message}", ex);
            }
        }


        public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request)
        {
            var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
            if (user is null) return null;

            return await CreateTokenResponse(user);
        }


        private async Task<ApplicationUsers?> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
        {
            var user = await dbContext.Users.FindAsync(userId);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }

            return user;
        }


        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }


        private async Task<string> GenerateAndSaveRefreshTokenAsync(ApplicationUsers user)
        {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await dbContext.SaveChangesAsync();
            return refreshToken;
        }


        private string CreateToken(ApplicationUsers user)
        {
            try
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                 };

                var securityKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));

                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

                var tokenDescriptor = new JwtSecurityToken
                (
                    issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                    audience: configuration.GetValue<string>("AppSettings:Audience"),
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(1),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Creating Token: {ex.Message}", ex);
            }
        }
    }
}
