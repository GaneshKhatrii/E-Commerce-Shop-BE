using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities
{
    public class Address : BaseEntity
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string AddressLine1 { get; set; } = string.Empty;
        public string? AddressLine2 { get; set; }
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty; 
        public string Country { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public AddressType AddressType { get; set; }
        public bool IsDefault { get; set; }

        // Navigation Property
        public User User { get; set; } = null!;
    }
}
