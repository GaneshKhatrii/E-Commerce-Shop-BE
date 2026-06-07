using ECommerce.Domain.Entities.Products;

namespace ECommerce.Domain.Entities.Orders
{
    public class OrderItem : BaseEntity
    {
        // Foreign keys
        public Guid OrderId { get; set; }
        public Guid ProductVariantId { get; set; }

        // Purchase snapshot 
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

        // Navigation properties
        public Order Order { get; set; } = null!;
        public ProductVariant ProductVariant { get; set; } = null!;
    }
}
