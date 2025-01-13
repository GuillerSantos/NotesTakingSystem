using Microsoft.EntityFrameworkCore;
using NTS.Server.Database.Configurations;
using NTS.Server.Database_Access.Configurations;
using NTS.Server.Domain.Entities;

namespace NTS.Server.Database.DatabaseContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ApplicationUsers> Users { get; set; }
        public DbSet<Notes> Notes { get; set; }
        public DbSet<FavoriteNotes> FavoriteNotes { get; set; }
        public DbSet<ImportantNotes> ImportantNotes { get; set; }
        public DbSet<SharedNotes> SharedNotes { get; set; }
        public DbSet<StarredNotes> StarredNotes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new UsersConfiguration());
            builder.ApplyConfiguration(new NotesConfiguration());
            builder.ApplyConfiguration(new FavoriteNotesConfiguration());
            builder.ApplyConfiguration(new ImportantNotesConfiguration());
            builder.ApplyConfiguration(new SharedNotesConfiguration());
            builder.ApplyConfiguration(new StarredNotesConfiguration());
        }
    }
}
