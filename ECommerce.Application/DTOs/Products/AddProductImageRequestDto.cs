namespace ECommerce.Application.DTOs.Products
{
    public class AddProductImageRequestDto
    {
        public Guid ProductVariantId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsPrimary { get; set; }
        public int DisplayOrder { get; set; }
    }
}
