using ECommerce.Application.Common;
using ECommerce.Application.DTOs.Admin;
using ECommerce.Application.DTOs.Products;
using ECommerce.Application.DTOs.User;

namespace ECommerce.Application.Interfaces.Admin
{
    public interface IAdminService
    {
        // Dashboard
        Task<ApiResponse<DashboardStatsResponseDto>> GetDashboardStatsAsync();

        // Users
        Task<ApiResponse<PagedResult<UserProfileResponseDto>>> GetAllUsersAsync(int pageNumber, int pageSize);

        // Products
        Task<ApiResponse<PagedResult<ProductResponseDto>>> GetAllProductsAsync(int pageNumber, int pageSize);

        // Orders
        Task<ApiResponse<PagedResult<AdminOrderListResponseDto>>> GetAllOrdersAsync(int pageNumber, int pageSize);

        Task<ApiResponse<AdminOrderDetailsResponseDto?>> GetOrderDetailsByIdAsync(Guid orderId);

        // Order Status Management Module
        Task<ApiResponse<string>> UpdateOrderStatusAsync(Guid orderId, UpdateOrderStatusRequestDto request);
    }
}
