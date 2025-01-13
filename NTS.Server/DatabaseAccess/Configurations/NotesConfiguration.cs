using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NTS.Server.Domain.Entities;

namespace NTS.Server.Database.DatabaseContext
{
    public class NotesConfiguration : IEntityTypeConfiguration<Notes>
    {
        public void Configure(EntityTypeBuilder<Notes> entityBuilder)
        {
            entityBuilder
                .ToTable("Notes");

            entityBuilder
                .HasOne(n => n.Users)
                .WithMany(u => u.Notes)
                .HasForeignKey(n => n.UserId);

            entityBuilder
                .HasKey(n => n.NoteId);

            entityBuilder
                .Property(n => n.Title)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("Title");

            entityBuilder
                .Property(n => n.Content)
                .HasMaxLength(255)
                .HasColumnName("Content");

            entityBuilder
                .Property(n => n.Priority)
                .HasMaxLength(50)
                .HasColumnName("Priority");

            entityBuilder
                .Property(n => n.UserId)
                .HasColumnName("UserId");

            entityBuilder
                .Property(n => n.CreatedAt)
                .HasColumnName("CreatedAt");
        }
    }
}