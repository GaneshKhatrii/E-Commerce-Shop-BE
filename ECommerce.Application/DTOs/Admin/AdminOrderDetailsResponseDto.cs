using ECommerce.Application.DTOs.Orders;

namespace ECommerce.Application.DTOs.Admin
{
    public class AdminOrderDetailsResponseDto
    {
        public Guid OrderId { get; set; }

        // Customer information
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        // Shipping address
        public string AddAddressLine1 { get; set; } = string.Empty;
        public string? AddressLine2 { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;

        // Order information
        public decimal TotalAmount { get; set; }
        public int StatusId { get; set; } 
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        // Products to pack
        public List<OrderItemResponseDto> OrderItems { get; set; } = new();
    }
}
