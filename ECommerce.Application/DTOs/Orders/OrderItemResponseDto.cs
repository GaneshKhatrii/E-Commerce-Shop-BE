namespace ECommerce.Application.DTOs.Orders
{
    public class OrderItemResponseDto
    {
        public Guid ProductVariantId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
