using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NTS.Server.Domain.Entities;

namespace NTS.Server.Database.Configurations
{
    public class UsersConfiguration : IEntityTypeConfiguration<ApplicationUsers>
    {
        public void Configure(EntityTypeBuilder<ApplicationUsers> entityBuilder)
        {
            entityBuilder
                .ToTable("ApplicationUsers");

            entityBuilder
                .HasKey(user => user.UserId);

            entityBuilder
                .Property(user => user.FullName)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("FullName");

            entityBuilder
                .Property(user => user.Email)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("Email");

            entityBuilder
                .Property(user => user.PasswordHash)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("PasswordHash");

            entityBuilder
                .Property(user => user.DateJoined)
                .IsRequired()
                .HasColumnType("datetime");

            entityBuilder
                .Property(user => user.PhoneNumber)
                .HasMaxLength(15)
                .HasColumnName("PhoneNumber");

            entityBuilder
                .Property(user => user.RecoveryEmail)
                .HasMaxLength(255)
                .HasColumnName("RecoveryEmail");

            entityBuilder
                .Property(user => user.Role)
                .IsRequired()
                .HasMaxLength(50);

            entityBuilder
                .Property(user => user.RefreshToken)
                .HasMaxLength(500)
                .IsRequired(false);

            entityBuilder
                .Property(user => user.RefreshTokenExpiryTime)
                .HasColumnType("datetime");
        }
    }
}
