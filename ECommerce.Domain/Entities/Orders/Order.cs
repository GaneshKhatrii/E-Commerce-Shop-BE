using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities.Orders
{
    public class Order : BaseEntity
    {
        // Customer, Foreign key
        public Guid UserId { get; set; }

        // Order details
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }

        // Shipping details snapshot
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string AddressLine1 { get; set; } = string.Empty;
        public string? AddressLine2 { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;

        // Navigation properties
        public User User { get; set; } = null!;
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
