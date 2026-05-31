using ECommerce.Application.Common;
using ECommerce.Application.DTOs.Products;

namespace ECommerce.Application.Interfaces.Products
{
    public interface IProductImageService
    {
        Task<ApiResponse<Guid?>> AddProductImageAsync(AddProductImageRequestDto request);
        Task<ApiResponse<List<ProductImageResponseDto>>> GetImagesByVariantIdAsync(Guid productVariantId);
    }
}
