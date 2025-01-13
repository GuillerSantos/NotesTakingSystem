using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NTS.Server.Domain.Entities;

namespace NTS.Server.Database_Access.Configurations
{
    public class FavoriteNotesConfiguration : IEntityTypeConfiguration<FavoriteNotes>
    {
        public void Configure(EntityTypeBuilder<FavoriteNotes> entityBuilder)
        {
            entityBuilder
                .ToTable("FavoriteNotes");

            entityBuilder
                .HasKey(f => f.FavoriteNoteId);

            entityBuilder
                .HasOne(f => f.Note)
                .WithMany()
                .HasForeignKey(f => f.NoteId)
                .OnDelete(DeleteBehavior.NoAction);

            entityBuilder
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entityBuilder
                .Property(f => f.CreatedAt)
                .IsRequired();

            entityBuilder
                .HasIndex(f => f.NoteId)
                .HasDatabaseName("IX_FavoriteNotes_NoteId");

            entityBuilder
                .HasIndex(f => f.UserId)
                .HasDatabaseName("IX_FavoriteNotes_UserId");
        }
    }
}
