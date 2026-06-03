using ECommerce.Application.Interfaces.Orders;
using ECommerce.Domain.Entities.Orders;
using ECommerce.Domain.Entities.Products;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories.Orders
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;
        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddCartAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
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

        public async Task AddCartItemAsync(CartItem cartItem)
        {
            await _context.CartItems.AddAsync(cartItem);
        }

        public async void RemoveCartItem(CartItem cartItem)
        {
            // Remove() is not async because it only marks entity for deletion in EF Core memory tracking
            // Actual database deletion happens when SaveChangesAsync() is called.
            // So dont use await here
            _context.CartItems.Remove(cartItem);
        }

        public async Task<CartItem?> GetCartItemAsync(Guid cartId, Guid productVariantId)
        {
            return await _context.CartItems.FirstOrDefaultAsync(x => x.CartId == cartId && x.ProductVariantId == productVariantId);
        }

        public async Task<CartItem?> GetCartItemByIdAsync(Guid cartItemId)
        {
            return await _context.CartItems.FirstOrDefaultAsync(x => x.Id == cartItemId);
        }

        public async Task<ProductVariant?> GetProductVariantByIdAsync(Guid productVariantId)
        {
            return await _context.ProductVariants.FirstOrDefaultAsync(x => x.Id == productVariantId);
        }

        public async Task<Inventory?> GetInventoryByVariantIdAsync(Guid productVariantId)
        {
            return await _context.Inventories.FirstOrDefaultAsync(x => x.ProductVariantId == productVariantId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
