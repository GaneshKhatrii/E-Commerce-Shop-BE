using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Configurations
{
    // IEntityTypeConfiguration<Address> is used to define Fluent API configurations for the Address entity.
    // IEntityTypeConfiguration forces to implement Configure() using EntityTypeBuilder object
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        // EntityTypeBuilder object provides methods like HasKey(), Property(), HasIndex(), and ToTable() to configure an entity’s database table 
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Addresses");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FullName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.AddressLine1)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.AddressLine2)
                .HasMaxLength(200);

            builder.Property(x => x.City)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.State)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Country)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.PostalCode)
                .IsRequired()
                .HasMaxLength(20);

            // HasConversion<string>() means in database, instead of  storing 1, 2 for Address types it stores Home, Office, Other
            builder.Property(x => x.AddressType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(x => x.IsDefault)
                .HasDefaultValue(false);

            // Relationship
            builder.HasOne(x => x.User)
                .WithMany(x => x.Addresses)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);  // automatically deletes addresses when user is deleted.
        }
    }
}
