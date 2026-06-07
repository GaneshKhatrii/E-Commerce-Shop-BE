using ECommerce.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Configurations.Orders
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Quantity)
                .IsRequired();

            // Allow 18 digits before decimal and 2 digits after decimal point
            builder.Property(x => x.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18, 2)");

            builder.Property(x => x.ImageUrl)
                .IsRequired()
                .HasMaxLength(500);

            // Relationships
            // Many OrderItems -> One Order
            builder.HasOne(x => x.Order)
                .WithMany(x => x.OrderItems)    // Order entity has a collection navigation property named OrderItems
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Many OrderItems -> One ProductVariant
            builder.HasOne(x => x.ProductVariant)
                .WithMany()                             // ProductVariant entity does not have a collection navigation property for OrderItems
                .HasForeignKey(x => x.ProductVariantId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
