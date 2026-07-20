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

        // COMMENTED THIS METHOD BECAUSE THE SAME METHOD IS ALREADY PRRESENT IN PRODUCT REPOSITORY AND IT IS NOT NECESSARY TO HAVE IT HERE. IF YOU WANT TO USE IT, YOU CAN CALL THE METHOD FROM PRODUCT REPOSITORY INSTEAD OF HAVING IT HERE.
        //Task<(List<Product> products, int totalRecords)> GetAllProductsAsync(int pageNumber, int pageSize);

        // Orders
        Task<(List<Order> orders, int totalRecords)> GetAllOrdersAsync(int pageNumber, int pageSize);

        Task<Order?> GetOrderDetailsByIdAsync(Guid orderId);

        // Order Status Management Module
        Task<Order?> GetOrderByIdForUpdateAsync(Guid orderId);
        Task SaveChangesAsync();

        // Product Module

        // ******> THESE METHODS ARE MOVED FROM PRODUCT MODULE TO ADMIN MODULE BECAUSE ONLY ADMIN CAN ADD PRODUCT, CATEGORY AND BRAND
        Task AddProductCategoryAsync(ProductCategory productCategory); 
        Task AddBrandAsync(Brand brand); 
        Task AddProductAsync(Product product);
        Task<(List<Product> products, int totalRecords)> GetProductsAsync(int pageNumber, int pageSize);
    }
}
