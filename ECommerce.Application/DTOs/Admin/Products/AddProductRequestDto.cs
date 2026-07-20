namespace ECommerce.Application.DTOs.Admin.Products
{
    public class AddProductRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid ProductCategoryId { get; set; }
        public Guid BrandId { get; set; }
    }
}
