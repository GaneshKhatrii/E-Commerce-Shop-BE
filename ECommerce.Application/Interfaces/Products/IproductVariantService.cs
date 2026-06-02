using ECommerce.Application.Common;
using ECommerce.Application.DTOs.Products;

namespace ECommerce.Application.Interfaces.Products
{
    public interface IproductVariantService
    {
        Task<ApiResponse<Guid?>> AddProductVariantAsync(AddProductVariantRequestDto request);
        Task<ApiResponse<List<ProductVariantResponseDto>>> GetProductVariantsAsync();
        Task<ApiResponse<List<ProductVariantResponseDto>>> GetVariantsByProductIdAsync(Guid productId);
    }
}
