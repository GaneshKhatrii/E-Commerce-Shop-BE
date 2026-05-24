using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Configurations
{
    // IEntityTypeConfiguration<User> is used to define Fluent API configurations for the User entity.
    // IEntityTypeConfiguration forces to implement Configure() using EntityTypeBuilder object
    public class UserConfiguration : IEntityTypeConfiguration<User>     
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(150);
            builder.HasIndex(x => x.Email)
                .IsUnique();
            builder.Property(x => x.PasswordHash)
                .IsRequired()
                .HasMaxLength(500);
            builder.Property(x => x.PhoneNumber)
                .HasMaxLength(20);
            builder.Property(x => x.Role)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);
            builder.Property(x => x.IsEmailVerified)
                .HasDefaultValue(false);
            builder.Property(x => x.EmailVerificationToken)
                .HasMaxLength(500);
        }
    }
}
