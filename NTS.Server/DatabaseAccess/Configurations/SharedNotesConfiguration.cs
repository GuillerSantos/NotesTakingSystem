using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NTS.Server.Domain.Entities;

namespace NTS.Server.Database_Access.Configurations
{
    public class SharedNotesConfiguration : IEntityTypeConfiguration<SharedNotes>
    {
        public void Configure(EntityTypeBuilder<SharedNotes> entityBuilder)
        {
            entityBuilder
                .ToTable("SharedNotes");

            entityBuilder
                .HasKey(s => s.SharedNoteId);

            entityBuilder
                .HasOne(s => s.Notes)
                .WithMany()
                .HasForeignKey(s => s.NoteId)
                .OnDelete(DeleteBehavior.NoAction);

            entityBuilder
                .HasOne(s => s.Users)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

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
