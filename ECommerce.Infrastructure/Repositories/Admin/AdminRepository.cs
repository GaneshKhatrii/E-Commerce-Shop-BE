using ECommerce.Application.Interfaces.Admin;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Orders;
using ECommerce.Domain.Entities.Products;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories.Admin
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _context;
        public AdminRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetTotalUsersAsync()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<int> GetTotalProductAsync()
        {
            return await _context.Products.CountAsync();
        }

        public async Task<int> GetTotalOrdersAsync()
        {
            return await _context.Orders.CountAsync();
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await _context.Orders
                .Select(x => (decimal?)x.TotalAmount).SumAsync() ?? 0;
        }

        public async Task<(List<User> users, int totalRecords)> GetAllUsersAsync(int pageNumber, int pageSize)
        {
            var totalRecords = await _context.Users.CountAsync();

            var users = await _context.Users
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (users, totalRecords);
        }

        public async Task<(List<Product> products, int totalRecords)> GetAllProductsAsync(int pageNumber, int pageSize)
        {
            var totalRecords = await _context.Products.CountAsync();

            var products = await _context.Products
                .Include(x => x.Brand)
                .Include(x => x.ProductCategory)
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (products, totalRecords);
        }

        public async Task<(List<Order> orders, int totalRecords)> GetAllOrdersAsync(int pageNumber, int pageSize)
        {
            var totalRecords = await _context.Orders.CountAsync();

            var orders = await _context.Orders
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (orders, totalRecords);
        }

        public async Task<Order?> GetOrderDetailsByIdAsync(Guid orderId)
        {
            return await _context.Orders
                .Include(x => x.OrderItems)
                    .ThenInclude(x => x.ProductVariant)
                        .ThenInclude(x => x.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == orderId);
        }

        // Order Status Management Module
        public async Task<Order?> GetOrderByIdForUpdateAsync(Guid orderId)
        {
            return await _context.Orders
                .FirstOrDefaultAsync(x => x.Id == orderId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
