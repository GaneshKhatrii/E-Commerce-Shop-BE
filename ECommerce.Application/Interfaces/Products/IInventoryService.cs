using ECommerce.Application.Common;
using ECommerce.Application.DTOs.Products;

namespace ECommerce.Application.Interfaces.Products
{
    public interface IInventoryService
    {
        Task<ApiResponse<string>> AddInventoryAsync(AddInventoryRequestDto request);
        Task<ApiResponse<String>> UpdateStockAsync(Guid productVariantId, UpdateInventoryStockRequestDto request);
        Task<ApiResponse<InventoryResponseDto?>> GetInventoryByVariantIdAsync(Guid productVariantId);
        Task<ApiResponse<PagedResult<InventoryResponseDto>>> GetInventoriesAsync(int pageNumber, int pageSize);
    }
}
