using ECommerce.Application.Common;
using ECommerce.Application.DTOs.Admin;
using ECommerce.Application.DTOs.Admin.Products;
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

        // Product Module

        // ******> THESE METHODS ARE MOVED FROM PRODUCT MODULE TO ADMIN MODULE BECAUSE ONLY ADMIN CAN ADD PRODUCT, CATEGORY AND BRAND
        Task<ApiResponse<Guid>> AddProductCategoryAsync(AddProductCategoryRequestDto request);
        Task<ApiResponse<Guid>> AddBrandAsync(AddBrandRequestDto request);
        Task<ApiResponse<Guid?>> AddProductAsync(AddProductRequestDto request);

        // Both admin and product module has this method but the data they return are different so we have to keep this method in both modules
        Task<ApiResponse<AdminProductResponseDto?>> GetProductByIdAsync(Guid productId);
        Task<ApiResponse<string>> UpdateProductAsync(Guid productId, UpdateProductRequestDto request);
        Task<ApiResponse<string>> UpdateProductStatusAsync(Guid productId, UpdateProductStatusDto request);
        //Task<ApiResponse<string>> UpdateProductVariantAsync(Guid productId, UpdateProductVariantRequestDto request);
    }
}
