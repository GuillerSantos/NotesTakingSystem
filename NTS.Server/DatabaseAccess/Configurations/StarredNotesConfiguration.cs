using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NTS.Server.Domain.Entities;

namespace NTS.Server.Database_Access.Configurations
{
    public class StarredNotesConfiguration : IEntityTypeConfiguration<StarredNotes>
    {
        public void Configure(EntityTypeBuilder<StarredNotes> entityBuilder)
        {
            entityBuilder
                .ToTable("StarredNotes");

            entityBuilder
                .HasKey(s => s.StarredNotesId);

            entityBuilder
                .HasOne(s => s.Notes)
                .WithMany()
                .HasForeignKey(s => s.NoteId)
                .OnDelete(DeleteBehavior.NoAction);

            entityBuilder
                .HasOne(s => s.Users)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            entityBuilder
                .Property(s => s.CreatedAt)
                .IsRequired();

            entityBuilder
                .HasIndex(s => s.NoteId)
                .HasDatabaseName("IX_FavoriteNotes_NoteId");

            entityBuilder
                .HasIndex(s => s.UserId)
                .HasDatabaseName("IX_FavoriteNotes_UserId");
        }
    }
}
