using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User?> GetUserByVerificationTokenAsync(string token)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.EmailVerificationToken == token);
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task SaveChangesAsync()
        {
            // Tracks and saves all entity changes
            await _context.SaveChangesAsync();
        }

        // User Management module 
        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task<Address> AddAddressAsync(Address address)
        {
            await _context.Addresses.AddAsync(address);
            return address;
        }

        public async Task<List<Address>> GetUserAddressesAsync(Guid userId)
        {
            return await _context.Addresses.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<Address?> GetAddressByIdAsync(Guid addressId)
        {
            return await _context.Addresses.FirstOrDefaultAsync(x => x.Id == addressId);
        }
    }
}
