using ECommerce.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Configurations.Products
{
    public class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
    {
        public void Configure(EntityTypeBuilder<Inventory> builder)
        {
            builder.ToTable("Inventories");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.AvailableStock)
                .IsRequired();

            // One-To-One Relationship
            builder.HasOne(x => x.ProductVariant)
                .WithOne(x => x.Inventory)
                .HasForeignKey<Inventory>(x => x.ProductVariantId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
