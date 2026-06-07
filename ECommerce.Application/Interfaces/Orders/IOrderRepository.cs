using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Orders;
using ECommerce.Domain.Entities.Products;

namespace ECommerce.Application.Interfaces.Orders
{
    public interface IOrderRepository
    {
        // Cart
        Task<Cart?> GetCartByUserIdAsync(Guid userId);

        // Address
        Task<Address?> GetAddressByIdAsync(Guid addressId, Guid userId);

        // Inventory
        Task<Inventory?> GetInventoryByVariantIdAsync(Guid productVariantId);

        // Order
        Task AddOrderAsync(Order order);

        // Order history
        Task<Order?> GetOrderByIdAsync(Guid orderId);
        Task<List<Order>> GetOrdersByUserIdAsync(Guid userId);

        // Order Status Management Module
        Task<Order?> GetOrderByIdForUpdateAsync(Guid orderId);

        Task SaveChangesAsync();
    }
}
