using ECommerce.Application.Common;
using ECommerce.Application.DTOs.Products;

namespace ECommerce.Application.Interfaces.Products
{
    public interface IproductVariantService
    {
        Task<ApiResponse<Guid?>> AddProductVariantAsync(AddProductVariantRequestDto request);
        Task<ApiResponse<PagedResult<ProductVariantResponseDto>>> GetProductVariantsAsync(int pageIndex, int pageSize);
        Task<ApiResponse<List<ProductVariantResponseDto>>> GetVariantsByProductIdAsync(Guid productId);
    }
}
