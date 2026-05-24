namespace ECommerce.Domain.Entities.Products
{
    public class Brand : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        // Navigation property for the related products
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
