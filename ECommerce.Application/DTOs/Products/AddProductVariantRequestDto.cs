namespace ECommerce.Application.DTOs.Products
{
    public class AddProductVariantRequestDto
    {
        public Guid ProductId { get; set; }
        public string Size { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
