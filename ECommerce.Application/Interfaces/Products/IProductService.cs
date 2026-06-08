using ECommerce.Application.Common;
using ECommerce.Application.DTOs.Products;

namespace ECommerce.Application.Interfaces.Products
{
    public interface IProductService
    {
        Task<ApiResponse<Guid>> AddProductCategoryAsync(AddProductCategoryRequestDto request); 
        Task<ApiResponse<Guid>> AddBrandAsync(AddBrandRequestDto request); 
        Task<ApiResponse<Guid?>> AddProductAsync(AddProductRequestDto request); 
        Task<ApiResponse<PagedResult<ProductResponseDto>>> GetProductsAsync(int pageNumber, int pageSize); 
        Task<ApiResponse<ProductResponseDto?>> GetProductByIdAsync(Guid productId); 
        Task<ApiResponse<List<ProductCategoryResponseDto>>> GetCategoriesAsync(); 
        Task<ApiResponse<List<BrandResponseDto>>> GetBrandsAsync();

        // Product Search & Filtering Module
        Task<ApiResponse<PagedResult<ProductVariantResponseDto>>> SearchProductsAsync(SearchProductsRequestDto request);
    }
}
