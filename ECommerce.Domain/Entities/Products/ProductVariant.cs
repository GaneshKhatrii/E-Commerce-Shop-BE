namespace ECommerce.Domain.Entities.Products
{
    public class ProductVariant : BaseEntity
    {
        public Guid ProductId { get; set; }
        public string Size { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
         
        // Navigation property
        public Product Product { get; set; } = null!;
        public ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
    }
}
