using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NTS.Server.Data;
using NTS.Server.Entities;
using NTS.Server.Entities.DTOs;
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
                var user = await dbContext.ApplicationUsers.FirstOrDefaultAsync(u => u.Email == request.Email);
                if (user is null)
                {
                    return null;
                }

                if (new PasswordHasher<ApplicationUsers>().VerifyHashedPassword
                    (user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
                {
                    return null;
                }

                return await CreateTokenResponse(user);
            }
            catch (Exception error)
            {
                throw new Exception($"Error Logging In User: {error.Message}");
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


        public async Task<ApplicationUsers?> RegisterUsersAsync(SignUpDto request, bool isAdmin)
        {
            try
            {
                if (await dbContext.ApplicationUsers.AnyAsync(u => u.Email == request.Email))
                    return null;

                var registerUser = new ApplicationUsers();

                var hashPassword = new PasswordHasher<ApplicationUsers>()
                    .HashPassword(registerUser, request.Password);

                registerUser.FullName = request.FullName;
                registerUser.Email = request.Email;
                registerUser.PasswordHash = hashPassword;
                registerUser.Role =  isAdmin ? "Admin" : "DefaultUser";
                registerUser.PhoneNumber = request.PhoneNumber;
                registerUser.RecoveryEmail = request.RecoveryEmail;
                registerUser.DateJoined = DateTime.UtcNow;


                dbContext.ApplicationUsers.Add(registerUser);
                await dbContext.SaveChangesAsync();
                return registerUser;
            }
            catch (Exception error)
            {
                throw new Exception($"Error Registering User: {error.Message}");
            }
        }


        public async Task<bool> RemoveAccountAsync(Guid userId)
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                var accountToRemove = await dbContext.ApplicationUsers.FindAsync(userId);

                if (accountToRemove is null)
                    return false;

                var relatedRecordsToRemove = new List<IQueryable<object>>
                {
                    dbContext.Notes.Where(note => note.UserId == userId),
                    dbContext.FavoriteNotes.Where(favNote => favNote.UserId == userId),
                    dbContext.ImportantNotes.Where(impNote => impNote.UserId == userId),
                    dbContext.SharedNotes.Where(sharedNote => sharedNote.UserId == userId),
                    dbContext.StarredNotes.Where(starNote => starNote.UserId == userId),
                };

                foreach (var records in relatedRecordsToRemove)
                {
                    var recordsList = records.ToList();
                    dbContext.RemoveRange(records);
                }

                dbContext.ApplicationUsers.Remove(accountToRemove);

                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception error)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error Removing Account: {error.Message}");
            }
        }


        public async Task<IEnumerable<UsersDto>> GetAllUsersAccounts(int page, int pageSize)
        {
            try
            {
                return await dbContext.ApplicationUsers
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
            catch (Exception error)
            {
                throw new Exception($"Error Retrieving All Users: {error.Message}");
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
            var user = await dbContext.ApplicationUsers.FindAsync(userId);
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
            catch (Exception error)
            {
                throw new Exception($"Error Creating Token: {error.Message}");
            }
        }
    }
}