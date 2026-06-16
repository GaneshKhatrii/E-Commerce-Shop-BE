namespace ECommerce.Application.DTOs.Admin
{
    public class AdminOrderListResponseDto
    {
        public Guid OrderId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public int StatusId { get; set; } 
        public string StatusName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
