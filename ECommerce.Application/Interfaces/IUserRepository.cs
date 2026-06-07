using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByVerificationTokenAsync(string token);
        Task AddUserAsync(User user);
        Task SaveChangesAsync();

        // User Management module 
        Task<User?> GetUserByIdAsync(Guid userId);
        Task<Address> AddAddressAsync(Address address);
        Task<List<Address>> GetUserAddressesAsync(Guid userId);
        Task<Address?> GetAddressByIdAsync(Guid userId, Guid addressId);
        void DeleteAddressAsync(Address address);
    }
}
