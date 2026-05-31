namespace ECommerce.Domain.Entities.Products
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid ProductCategoryId { get; set; }
        public Guid BrandId { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public ProductCategory ProductCategory { get; set; } = null!;
        public Brand Brand { get; set; } = null!;
        public ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
    }
}
