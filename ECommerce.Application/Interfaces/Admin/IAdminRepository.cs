using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Orders;
using ECommerce.Domain.Entities.Products;

namespace ECommerce.Application.Interfaces.Admin
{
    public interface IAdminRepository
    {
        // Dashboard Statistics
        Task<int> GetTotalUsersAsync();
        Task<int> GetTotalProductAsync();
        Task<int> GetTotalOrdersAsync();
        Task<decimal> GetTotalRevenueAsync();

        // Users
        Task<(List<User> users, int totalRecords)> GetAllUsersAsync(int pageNumber, int pageSize);

        // Products
        Task<(List<Product> products, int totalRecords)> GetAllProductsAsync(int pageNumber, int pageSize);

        // Orders
        Task<(List<Order> orders, int totalRecords)> GetAllOrdersAsync(int pageNumber, int pageSize);

        Task<Order?> GetOrderDetailsByIdAsync(Guid orderId);

        // Order Status Management Module
        Task<Order?> GetOrderByIdForUpdateAsync(Guid orderId);
        Task SaveChangesAsync();
    }
}
