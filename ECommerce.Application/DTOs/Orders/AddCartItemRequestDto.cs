namespace ECommerce.Application.DTOs.Orders
{
    public class AddCartItemRequestDto
    {
        public Guid ProductVariantId { get; set; }
        public int Quantity { get; set; }
    }
}
