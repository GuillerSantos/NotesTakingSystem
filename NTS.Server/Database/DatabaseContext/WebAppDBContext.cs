using Microsoft.EntityFrameworkCore;
using NTS.Server.Database.Configurations;
using NTS.Server.Domain.Entities;

namespace NTS.Server.Database.DatabaseContext
{
    public class WebAppDBContext : DbContext
    {
        public WebAppDBContext(DbContextOptions<WebAppDBContext> options) : base(options) { }

        public DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UsersConfiguration());
        }
    }
}
