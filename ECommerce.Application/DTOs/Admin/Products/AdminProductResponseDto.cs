namespace ECommerce.Application.DTOs.Admin.Products
{
    public class AdminProductResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid productCategoryId { get; set; } 
        public Guid brandId { get; set; } 
        public bool IsActive { get; set; }
    }
}
