namespace ECommerce.Application.DTOs.Admin.Products
{
    public class UpdateProductVariantRequestDto
    {
        public string Size { get; set; } = string.Empty; 
        public string Color { get; set; } = string.Empty; 
        public decimal Price { get; set; }
    }
}
