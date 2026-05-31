namespace ECommerce.Application.DTOs.Products
{
    public class ProductImageResponseDto
    {
        public Guid Id { get; set; }
        public Guid ProductVariantId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsPrimary { get; set; }
        public int DisplayOrder { get; set; }
    }
}
