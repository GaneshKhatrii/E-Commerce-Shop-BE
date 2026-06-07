using ECommerce.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Configurations.Orders
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(x => x.Id);

            // 18 digits before decimal and 2 digits after decimal point
            builder.Property(x => x.TotalAmount)
                .IsRequired()
                .HasColumnType("decimal(18, 2)"); 

            builder.Property(x => x.Status)
                .IsRequired();

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
                .HasMaxLength(100);

            // Relationships
            // One User -> Many Orders
            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict); // If user deleted then.....

            // One Order -> Many OrderItems
            builder.HasMany(x => x.OrderItems)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // If Order gets deleted then delete OrderItems too
        }
    }
}
