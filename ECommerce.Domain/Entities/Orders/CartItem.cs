using ECommerce.Domain.Entities.Products;

namespace ECommerce.Domain.Entities.Orders
{
    public class CartItem : BaseEntity
    {
        // Foreign Keys
        public Guid CartId { get; set; }
        public Guid ProductVariantId { get; set; }
        public int Quantity { get; set; }

        // Navigation properties
        public Cart Cart { get; set; } = null!;
        public ProductVariant ProductVariant { get; set; } = null!;
    }
}
