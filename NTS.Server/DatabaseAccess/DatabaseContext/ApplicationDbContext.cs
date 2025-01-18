using Microsoft.EntityFrameworkCore;
using NTS.Server.Entities;

namespace NTS.Server.Data
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // ApplicationUsers
            modelBuilder.Entity<ApplicationUsers>()
                .HasKey(a => a.UserId);


            // NotesBase
            modelBuilder.Entity<Notes>(entity =>
            {
                entity.HasOne(e => e.User)
                      .WithMany(u => u.Notes)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.UserId)
                       .HasDatabaseName("IX_NotesBase_UserId");
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
                      .OnDelete(DeleteBehavior.Cascade);
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
                      .OnDelete(DeleteBehavior.Cascade);
            });


            // SharedNotes
            modelBuilder.Entity<SharedNotes>(entity =>
            {
                entity.HasOne(e => e.Note)
                      .WithMany()
                      .HasForeignKey(e => e.NoteId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.SharedWithUser)
                      .WithMany()
                      .HasForeignKey(e => e.SharedWithUserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });


            // StarredNotes (Separate From SharedNotes Configuration)
            modelBuilder.Entity<StarredNotes>(entity =>
            {
                entity.HasOne(e => e.Note)
                      .WithMany()
                      .HasForeignKey(e => e.NoteId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
