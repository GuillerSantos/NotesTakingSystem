using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NTS.Server.Domain.Entities;

namespace NTS.Server.Database_Access.Configurations
{
    public class ImportantNotesConfiguration : IEntityTypeConfiguration<ImportantNotes>
    {
        public void Configure(EntityTypeBuilder<ImportantNotes> entityBuilder)
        {
            entityBuilder.ToTable("ImportantNotes");

            entityBuilder.HasKey(i => i.ImportantNoteId);

            entityBuilder
                .HasOne(i => i.Notes)
                .WithMany()
                .HasForeignKey(i => i.NoteId)
                .OnDelete(DeleteBehavior.Cascade);

            entityBuilder
                .HasOne(i => i.Users)
                .WithMany()
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entityBuilder
                .Property(i => i.CreatedAt)
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
