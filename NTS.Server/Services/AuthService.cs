﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NTS.Server.Data;
using NTS.Server.DTOs;
using NTS.Server.Entities;
using NTS.Server.Services.Contracts;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Transactions;

namespace NTS.Server.Services
{
    public class AuthService : IAuthService
    {
        #region Fields

        private readonly ApplicationDbContext dbContext;
        private readonly INotesService noteService;
        private readonly IFavoriteNoteService favoriteNoteService;
        private readonly IImpotantNotesService impotantNotesService;
        private readonly ISharedNotesService sharedNotesService;
        private readonly IStarredNotesService starredNotesService;
        private readonly IConfiguration configuration;

        #endregion Fields

        #region Public Constructors

        public AuthService(ApplicationDbContext dbContext, IConfiguration configuration,
            INotesService notesService, IFavoriteNoteService favoriteNoteService,
            IImpotantNotesService impotantNotesService, ISharedNotesService sharedNotesService,
            IStarredNotesService starredNotesService)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.noteService = notesService ?? throw new ArgumentNullException(nameof(notesService));
            this.favoriteNoteService = favoriteNoteService ?? throw new ArgumentNullException(nameof(favoriteNoteService));
            this.impotantNotesService = impotantNotesService ?? throw new ArgumentNullException(nameof(impotantNotesService));
            this.sharedNotesService = sharedNotesService ?? throw new ArgumentNullException(nameof(sharedNotesService));
            this.starredNotesService = starredNotesService ?? throw new ArgumentNullException(nameof(starredNotesService));
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<TokenResponseDto?> LoginUsersAsync(LoginDto request)
        {
            var user = await dbContext.ApplicationUsers.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user is null)
            {
                Console.WriteLine("User Not Found");
                return null;
            }

            var passwordVerifivationResult = new PasswordHasher<ApplicationUsers>().VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (passwordVerifivationResult == PasswordVerificationResult.Failed)
            {
                Console.WriteLine("Password Verification Failed");
                return null;
            }

            if (string.IsNullOrEmpty(user.RefreshToken))
            {
                user.RefreshToken = await GenerateAndSaveRefreshTokenAsync(user);
                await dbContext.SaveChangesAsync();
            }

            return await CreateTokenResponse(user);
        }

        public async Task<bool> LogoutAsync(Guid userId)
        {
            var currentUser = await dbContext.ApplicationUsers.FindAsync(userId);

            if (currentUser is null) return false;

            currentUser.RefreshToken = null!;
            currentUser.RefreshTokenExpiryTime = null;
            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<ApplicationUsers?> RegisterUsersAsync(RegisterDto request, bool isAdmin)
        {
            if (await dbContext.ApplicationUsers.AnyAsync(u => u.Email == request.Email))
                return null;

            var registerUser = new ApplicationUsers();

            var hashPassword = new PasswordHasher<ApplicationUsers>()
                .HashPassword(registerUser, request.Password);

            registerUser.FullName = request.FullName;
            registerUser.Email = request.Email;
            registerUser.PasswordHash = hashPassword;
            registerUser.Role = isAdmin ? "Admin" : "DefaultUser";
            registerUser.PhoneNumber = request.PhoneNumber;
            registerUser.RecoveryEmail = request.RecoveryEmail;
            registerUser.DateJoined = DateTime.UtcNow;

            dbContext.ApplicationUsers.Add(registerUser);
            await dbContext.SaveChangesAsync();
            return registerUser;
        }

        public async Task<bool> RemoveAccountAsync(Guid userId, Guid noteId)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var accountToRemove = await dbContext.ApplicationUsers.FindAsync(userId);

            if (accountToRemove is null) return false;

            await noteService.RemoveNoteAsync(userId, noteId);
            await favoriteNoteService.UnmarkNoteAsFavoriteAsync(userId);
            await impotantNotesService.UnmarkNoteAsImportantAsync(userId);
            await sharedNotesService.UnmarkNoteAsSharedAsync(userId);
            await starredNotesService.UnmarkNoteAsStarredAsync(userId);

            dbContext.ApplicationUsers.Remove(accountToRemove);

            await dbContext.SaveChangesAsync();
            transactionScope.Complete();

            return true;
        }

        public async Task<IEnumerable<UsersDto>> GetAllUsersAccounts(int page, int pageSize)
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

        public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request)
        {
            var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
            if (user is null) return null;

            return await CreateTokenResponse(user);
        }

        #endregion Public Methods

        #region Private Methods

        private async Task<TokenResponseDto> CreateTokenResponse(ApplicationUsers? user)
        {
            return new TokenResponseDto()
            {
                AccessToken = CreateToken(user!),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user!)
            };
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
            Console.WriteLine($"Generated RefreshToken: {refreshToken}");
            return refreshToken;
        }

        private string CreateToken(ApplicationUsers user)
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

        #endregion Private Methods
    }
}