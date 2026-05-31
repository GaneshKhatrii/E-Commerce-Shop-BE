using ECommerce.Application.Common;
using ECommerce.Application.DTOs.Products;
using ECommerce.Application.Interfaces.Products;
using ECommerce.Domain.Entities.Products;

namespace ECommerce.Infrastructure.Services.Products
{
    public class ProductVariantService : IproductVariantService
    {
        private readonly IproductVariantRepository _productVariantRepository;
        public ProductVariantService(IproductVariantRepository productVariantRepository)
        {
            _productVariantRepository = productVariantRepository;
        }
        public async Task<ApiResponse<Guid?>> AddProductVariantAsync(AddProductVariantRequestDto request)
        {
            var product = await _productVariantRepository.GetProductByIdAsync(request.ProductId);

            if (product == null)
            {
                return new ApiResponse<Guid?>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Product not found",
                    Data = null
                };
            }

            var productVariant = new ProductVariant
            {
                ProductId = request.ProductId,
                Size = request.Size,
                Color = request.Color,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
            };

            await _productVariantRepository.AddProductVariantAsync(productVariant);
            await _productVariantRepository.SaveChangesAsync();

            return new ApiResponse<Guid?>
            {
                Success = true,
                StatusCode = 201,
                Message = "Product variant added successfully",
                Data = productVariant.Id
            };
        }

        public async Task<ApiResponse<PagedResult<ProductVariantResponseDto>>> GetProductVariantsAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;

            if (pageSize < 1) pageSize = 5;

            var (variants, totalRecords) = await _productVariantRepository.GetProductVariantsAsync(pageNumber, pageSize);

            var variantsList = variants.Select(x => new ProductVariantResponseDto
            {
                Id = x.Id,
                ProductId = x.ProductId,
                ProductName = x.Product.Name,
                Size = x.Size,
                Color = x.Color,
                Price = x.Price,
                StockQuantity = x.StockQuantity,

            }).ToList();

            return new ApiResponse<PagedResult<ProductVariantResponseDto>>
            {
                Success = true,
                StatusCode = 200,
                Message = variantsList.Any() ? "Product variants retrieved successfully" : "No variants found",
                Data = new PagedResult<ProductVariantResponseDto>
                {
                    Items = variantsList,
                    TotalRecords = totalRecords,
                }
            };
        }

        public async Task<ApiResponse<List<ProductVariantResponseDto>>> GetVariantsByProductIdAsync(Guid productId)
        {
            var variants = await _productVariantRepository.GetVariantsByProductIdAsync(productId);
            var variantsList = variants.Select(x => new ProductVariantResponseDto
            {
                Id = x.Id,
                ProductId = x.ProductId,
                ProductName = x.Product.Name,
                Size = x.Size,
                Color = x.Color,
                Price = x.Price,
                StockQuantity = x.StockQuantity,

            }).ToList();

            return new ApiResponse<List<ProductVariantResponseDto>>
            {
                Success = true,
                StatusCode = 200,
                Message = variantsList.Any() ? "Product variants retrieved successfully" : "No variants found for the specified product",
                Data = variantsList
            };
        }
    }
}
