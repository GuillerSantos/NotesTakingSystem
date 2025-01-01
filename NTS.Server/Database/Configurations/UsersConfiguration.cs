using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NTS.Server.Domain.Entities;

namespace NTS.Server.Database.Configurations
{
    public class UsersConfiguration : IEntityTypeConfiguration<Users>
    {

        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.HasKey(user => user.UserId);

            builder.Property(user => user.Username)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property (user => user.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property (user => user.PasswordHash)
                .HasMaxLength(255);

            builder.Property(user => user.PasswordSalt)
                .HasMaxLength(255);

            builder.Property(user => user.DateJoined);

            builder.Property(user => user.PhoneNumber);

            builder.Property(user => user.RecoveryEmail);
        }
    }
}
