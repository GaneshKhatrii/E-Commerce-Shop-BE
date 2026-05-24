using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        // Admin/User
        public UserRole Role { get; set; } = UserRole.User;

        // Email verification status
        public bool IsEmailVerified { get; set; } = false;

        // Verification token
        public string? EmailVerificationToken { get; set; }

        // Token expiration
        public DateTime? EmailVerificationTokenExpiry { get; set; }

        // Navigation property
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
    }
}
