using ECommerce.Application.Common;
using ECommerce.Application.DTOs.Products;
using ECommerce.Application.Interfaces.Products;
using ECommerce.Domain.Entities.Products;

namespace ECommerce.Infrastructure.Services.Products
{
    public class ProductImageService : IProductImageService
    {
        private readonly IProductImageRepository _productImageRepository;
        public ProductImageService(IProductImageRepository productImageRepository)
        {
            _productImageRepository = productImageRepository;
        }

        public async Task<ApiResponse<Guid?>> AddProductImageAsync(AddProductImageRequestDto request)
        {
            var productVariant = await _productImageRepository.GetProductVariantByIdAsync(request.ProductVariantId);

            if (productVariant == null)
            {
                return new ApiResponse<Guid?>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Product variant not found.",
                    Data = null
                };
            }

            var productImage = new ProductImage
            {
                ProductVariantId = request.ProductVariantId,
                ImageUrl = request.ImageUrl,
                IsPrimary = request.IsPrimary,
                DisplayOrder = request.DisplayOrder
            };

            await _productImageRepository.AddProductImageAsync(productImage);
            await _productImageRepository.SaveChangesAsync();

            return new ApiResponse<Guid?>
            {
                Success = true,
                StatusCode = 201,
                Message = "Product image added successfully",
                Data = productImage.Id
            };
        }

        public async Task<ApiResponse<List<ProductImageResponseDto>>> GetImagesByVariantIdAsync(Guid productVariantId)
        {
            var images = await _productImageRepository.GetImagesByVariantIdAsync(productVariantId);

            var imagesList = images.Select(x => new ProductImageResponseDto
            {
                Id = x.Id,
                ProductVariantId = x.ProductVariantId,
                ImageUrl = x.ImageUrl,
                IsPrimary = x.IsPrimary,
                DisplayOrder = x.DisplayOrder,
            }).ToList();

            return new ApiResponse<List<ProductImageResponseDto>>
            {
                Success = true,
                StatusCode = 200,
                Message = imagesList.Any() ? "Product images retrieved successfully" : "No images found",
                Data = imagesList
            };
        }
    }
}
