using Microsoft.EntityFrameworkCore;
using NTS.Server.Entities;

namespace NTS.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ApplicationUsers> ApplicationUsers { get; set; }
        public DbSet<NotesBase> NotesBase { get; set; }
        public DbSet<FavoriteNotes> FavoriteNotes { get; set; }
        public DbSet<ImportantNotes> ImportantNotes { get; set; }
        public DbSet<SharedNotes> SharedNotes { get; set; }
        public DbSet<StarredNotes> StarredNotes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUsers>()
                .HasKey(a => a.UserId);

            // NotesBase
            modelBuilder.Entity<NotesBase>(entity =>
            {
                entity.HasOne(e => e.ApplicationUser)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // FavoriteNotes
            modelBuilder.Entity<FavoriteNotes>(entity =>
            {
                entity.HasOne(e => e.Note)
                      .WithMany()
                      .HasForeignKey(e => e.NoteId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.ApplicationUser)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ImportantNotes
            modelBuilder.Entity<ImportantNotes>(entity =>
            {
                entity.HasOne(e => e.Note)
                      .WithMany()
                      .HasForeignKey(e => e.NoteId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.ApplicationUser)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // SharedNotes
            modelBuilder.Entity<SharedNotes>(entity =>
            {
                entity.HasOne(e => e.Note)
                      .WithMany()
                      .HasForeignKey(e => e.NoteId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.ApplicationUser)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.SharedWithUser)
                      .WithMany()
                      .HasForeignKey(e => e.SharedWithUserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // StarredNotes (Separate from SharedNotes configuration)
            modelBuilder.Entity<StarredNotes>(entity =>
            {
                entity.HasOne(e => e.Note)
                      .WithMany()
                      .HasForeignKey(e => e.NoteId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.ApplicationUser)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

    }
}
