namespace ECommerce.Domain.Entities.Products
{
    public class Inventory : BaseEntity
    {
        // Foreign key 
        public Guid ProductVariantId { get; set; }
        public int AvailableStock { get; set; }

        // Navigation property
        public ProductVariant ProductVariant { get; set; } = null!;
    }
}
