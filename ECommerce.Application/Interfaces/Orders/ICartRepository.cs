using ECommerce.Domain.Entities.Orders;
using ECommerce.Domain.Entities.Products;

namespace ECommerce.Application.Interfaces.Orders
{
    public interface ICartRepository
    {
        // Create new cart
        Task AddCartAsync(Cart cart);

        // Get user's cart
        Task<Cart?> GetCartByUserIdAsync(Guid userId);

        // Add item into cart
        Task AddCartItemAsync(CartItem cartItem);

        // Remove cart item
        void RemoveCartItem(CartItem cartItem);

        // Find existing cart item
        Task<CartItem?> GetCartItemAsync(Guid cartId, Guid productVariantId);

        // Get cart item by id
        Task<CartItem?> GetCartItemByIdAsync(Guid cartItemId);

        // Validate product variant exists or not before adding to cart
        Task<ProductVariant?> GetProductVariantByIdAsync(Guid productVariantId);

        // Validate inventory for the product variant before adding to cart
        Task<Inventory?> GetInventoryByVariantIdAsync(Guid productVariantId);

        // Save database changes
        Task SaveChangesAsync();
    }
}
