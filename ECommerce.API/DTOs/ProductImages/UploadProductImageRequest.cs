namespace ECommerce.API.DTOs.ProductImages
{
    public class UploadProductImageRequest
    {
        public Guid ProductVariantId { get; set; }
        public IFormFile Image { get; set; } = null!;
        public bool IsPrimary { get; set; }
        public int DisplayOrder { get; set; }
    }
}
