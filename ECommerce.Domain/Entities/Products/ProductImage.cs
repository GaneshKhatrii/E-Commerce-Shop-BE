namespace ECommerce.Domain.Entities.Products
{
    public class ProductImage : BaseEntity
    {
        // Foreign Key
        public Guid ProductVariantId { get; set; }

        // Image Path / URL
        public string ImageUrl { get; set; } = string.Empty;

        // Main Thumbnail Image
        public bool IsPrimary { get; set; }

        // Image Ordering
        public int DisplayOrder { get; set; }

        // Navigation Property
        public ProductVariant ProductVariant { get; set; } = null!;
    }
}
