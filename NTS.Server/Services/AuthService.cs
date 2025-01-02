using Microsoft.EntityFrameworkCore;
using NTS.Server.Database.DatabaseContext;
using NTS.Server.Domain.DTOs;
using NTS.Server.Domain.Entities;
using NTS.Server.Securities;
using NTS.Server.Services.Contracts;

namespace NTS.Server.Services
{
    public class AuthService : IAuthService
    {
        private readonly WebAppDBContext dBContext;
        private readonly TokenAndPasswordHash tokenAndPasswordHash;
        private readonly IConfiguration configuration;

        public AuthService(WebAppDBContext dBContext, TokenAndPasswordHash tokenAndPasswordHash, IConfiguration configuration)
        {
            this.dBContext = dBContext;
            this.tokenAndPasswordHash = tokenAndPasswordHash;
            this.configuration = configuration;
        }
    }
}
