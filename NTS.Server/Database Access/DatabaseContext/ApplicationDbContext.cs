using Microsoft.EntityFrameworkCore;
using NTS.Server.Database.Configurations;
using NTS.Server.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace NTS.Server.Database.DatabaseContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ApplicationUsers> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new UsersConfiguration());
        }
    }
}
