namespace ECommerce.Application.DTOs.Orders
{
    public class OrderResponseDto
    {
        public Guid OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<OrderItemResponseDto> OrderItems { get; set; } = new List<OrderItemResponseDto>();
    }
}
