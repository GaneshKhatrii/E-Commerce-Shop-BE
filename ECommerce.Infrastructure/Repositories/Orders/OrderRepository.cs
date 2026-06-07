using ECommerce.Application.Interfaces.Orders;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Orders;
using ECommerce.Domain.Entities.Products;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories.Orders
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Cart?> GetCartByUserIdAsync(Guid userId)
        {
            return await _context.Carts
                .Include(x => x.CartItems)
                    .ThenInclude(x => x.ProductVariant)
                        .ThenInclude(x => x.Product)
                .Include(x => x.CartItems)
                    .ThenInclude(x => x.ProductVariant)
                        .ThenInclude(x => x.ProductImages)
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<Address?> GetAddressByIdAsync(Guid addressId, Guid userId)
        {
            return await _context.Addresses
                .FirstOrDefaultAsync(x => x.Id == addressId && x.UserId == userId);
        }

        public async Task<Inventory?> GetInventoryByVariantIdAsync(Guid productVariantId)
        {
            return await _context.Inventories
                .FirstOrDefaultAsync(x => x.ProductVariantId == productVariantId);
        }

        public async Task AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public async Task<Order?> GetOrderByIdAsync(Guid orderId)
        {
            return await _context.Orders
                .Include(x => x.OrderItems)
                    .ThenInclude(x => x.ProductVariant)
                        .ThenInclude(x => x.Product)
                .Include(x => x.OrderItems)
                    .ThenInclude(x => x.ProductVariant)
                        .ThenInclude(x => x.ProductImages)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == orderId);
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(Guid userId)
        {
            return await _context.Orders
                .Include(x => x.OrderItems)
                    .ThenInclude(x => x.ProductVariant)
                        .ThenInclude(x => x.Product)
                .Include(x => x.OrderItems)
                    .ThenInclude(x => x.ProductVariant)
                        .ThenInclude(x => x.ProductImages)
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)        // Newest First
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
