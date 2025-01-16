using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NTS.Server.Entities;

namespace NTS.Server.Database.DatabaseContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ApplicationUsers> ApplicationUsers { get; set; }
        public DbSet<Notes> Notes { get; set; }
        public DbSet<FavoriteNotes> FavoriteNotes { get; set; }
        public DbSet<ImportantNotes> ImportantNotes { get; set; }
        public DbSet<SharedNotes> SharedNotes { get; set; }
        public DbSet<StarredNotes> StarredNotes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ApplicationUsers
            builder.Entity<ApplicationUsers>()
                .ToTable("ApplicationUsers");

            builder.Entity<ApplicationUsers>()
                .HasKey(user => user.UserId);

            // Notes
            builder.Entity<Notes>()
                .ToTable("Notes");

            builder.Entity<Notes>()
                .HasKey(n => n.NoteId);

            builder.Entity<Notes>()
                .HasOne(n => n.ApplicationUser)
                .WithMany(u => u.Notes)
                .HasForeignKey(n => n.UserId);

            // FavoriteNotes
            builder.Entity<FavoriteNotes>()
                .HasKey(f => f.FavoriteNoteId);

            builder.Entity<FavoriteNotes>()
                .ToTable("FavoriteNotes");

            builder.Entity<FavoriteNotes>()
                .HasOne(f => f.Note)
                .WithMany()
                .HasForeignKey(f => f.NoteId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<FavoriteNotes>()
                .HasOne(f => f.ApplicationUser)
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ImportantNotes
            builder.Entity<ImportantNotes>()
                .ToTable("ImportantNotes");

            builder.Entity<ImportantNotes>()
                .HasKey(i => i.ImportantNoteId);

            builder.Entity<ImportantNotes>()
                .HasOne(i => i.ApplicationUser)
                .WithMany()
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.NoAction);  // Change this to NoAction or SetNull

            builder.Entity<ImportantNotes>()
                .HasOne(i => i.Note)
                .WithMany()
                .HasForeignKey(i => i.NoteId)
                .OnDelete(DeleteBehavior.Cascade);  // Keep or change to NoAction/SetNull

            // SharedNotes
            builder.Entity<SharedNotes>()
                .ToTable("SharedNotes");

            builder.Entity<SharedNotes>()
                .HasKey(s => s.SharedNoteId);

            builder.Entity<SharedNotes>()
                .HasOne(s => s.ApplicationUser)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SharedNotes>()
                .HasOne(s => s.Note)
                .WithMany()
                .HasForeignKey(s => s.NoteId)
                .OnDelete(DeleteBehavior.Cascade);

            // StarredNotes
            builder.Entity<StarredNotes>()
                .ToTable("StarredNotes");

            builder.Entity<StarredNotes>()
                .HasKey(s => s.StarredNotesId);

            builder.Entity<StarredNotes>()
                .HasOne(s => s.ApplicationUser)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<StarredNotes>()
                .HasOne(s => s.Note)
                .WithMany()
                .HasForeignKey(s => s.NoteId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
