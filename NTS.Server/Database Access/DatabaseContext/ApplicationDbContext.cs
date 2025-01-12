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
        public DbSet<Notes> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new UsersConfiguration());
            builder.ApplyConfiguration(new NotesConfiguration());

            builder.Entity<Notes>()
                .HasOne(n => n.Users)
                .WithMany(u => u.Notes)
                .HasForeignKey(n => n.UserId);
        }
    }
}
