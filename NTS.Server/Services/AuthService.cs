using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NTS.Server.Database.DatabaseContext;
using NTS.Server.Domain.DTOs;
using NTS.Server.Domain.Entities;
using NTS.Server.Services.Contracts;
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


        public async Task<IEnumerable<UsersDto>> GetAllUsersAccounts()
        {
            try
            {
                var accounts = await dbContext.Users
                    .Select(u => new UsersDto
                    {
                        UserId = u.UserId,
                        FullName = u.FullName,
                        Email = u.Email,
                        Role = u.Role,
                        DateJoined = u.DateJoined,
                    })
                    .ToListAsync();

                return accounts;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error Retrieving All Users: {ex.Message}");
            }
        }

        public async Task<ApplicationUsers> RegisterUsersAsync(SignUpDto request, string role = "User")
        {
            if (request.Password != request.ConfirmPassword) throw new Exception("Password Do Not Match");

            var existingUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (existingUser != null) throw new Exception("User With This Email Already Exists");

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new ApplicationUsers
            {
                FullName = request.FullName,
                Email = request.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                PhoneNumber = request.PhoneNumber,
                RecoveryEmail = request.RecoveryEmail,
                DateJoined = DateTime.UtcNow,
                Role = role
            };

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<string> LoginUsersAsync(LoginDto request)
        {
            var users = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (users == null) throw new Exception("User With this Email Not Found");

            if (!VerifyPasswordHash(request.Password, users.PasswordHash, users.PasswordSalt))
                throw new Exception("Incorrect Password");

            var token = CreateToken(users.UserId, users.Email);

            return token;
        }


        public string CreateToken(Guid userId, string username)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AppSettings:Token"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            passwordSalt = hmac.Key;
        }


        public bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(storedHash);
        }
    }
}
