namespace ECommerce.Application.DTOs.Products
{
    public class AddInventoryRequestDto
    {
        public Guid ProductVariantId { get; set; }
        public int AvailableStock { get; set; }
    }
}
