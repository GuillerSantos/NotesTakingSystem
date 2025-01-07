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


        public async Task<ApplicationUsers> RegisterUsersAsync(SignUpDto request, string role = "User")
        {
            try
            {
                if (request.Password != request.ConfirmPassword)
                    throw new Exception("Password Do Not Match");

                var existingUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

                if (existingUser != null)
                    throw new Exception("User With This Email Already Exists");

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
            catch (Exception ex)
            {
                throw new Exception($"Error Registering User: {ex.Message}", ex);
            }
        }


        public async Task<string> LoginUsersAsync(LoginDto request)
        {
            try
            {
                var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
                if (user == null)
                    throw new InvalidOperationException("User With this Email Not Found");

                if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                    throw new InvalidOperationException("Incorrect Password");

                var token = GenerateJwtToken(user.UserId, user.Email, user.FullName, user.Role);

                return token;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Logging In User: {ex.Message}", ex);
            }
        }


        public string GenerateJwtToken(Guid userId, string email, string fullName, string role)
        {
            try
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Name, fullName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, email),
                    new Claim(ClaimTypes.Role, role)
                };


                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AppSettings:Token"]!));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddSeconds(5),
                    SigningCredentials = credentials
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Creating Token: {ex.Message}", ex);
            }
        }


        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            passwordSalt = hmac.Key;
        }


        public bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            try
            {
                using var hmac = new HMACSHA512(storedSalt);
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(storedHash);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Verifying Password Hash: {ex.Message}", ex);
            }
        }
    }
}
