using ECommerce.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Configurations.Orders
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("Carts");

            builder.HasKey(x => x.Id);

            // Relationships
            // One User -> One Cart, One-to-one relationship 
            builder.HasOne(x => x.User)
                .WithOne()
                .HasForeignKey<Cart>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Below code is optional because the EF Core will automatically create a unique index for the foreign key in a one-to-one relationship(Code written in above line), but it's added here for clarity.
            builder.HasIndex(x => x.UserId)
                .IsUnique();

            // One Cart -> Many CartItems, One-to-many relationship
            builder.HasMany(x => x.CartItems)
                .WithOne(x => x.Cart)
                .HasForeignKey(x => x.CartId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
