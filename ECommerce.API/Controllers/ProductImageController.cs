using ECommerce.API.DTOs.ProductImages;
using ECommerce.Application.DTOs.Products;
using ECommerce.Application.Interfaces.Products;
using ECommerce.Application.Interfaces.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        private readonly IProductImageService _productImageService;
        private readonly IFileStorageService _fileStorageService;

        public ProductImageController(
            IProductImageService productImageService,
            IFileStorageService fileStorageService)
        {
            _productImageService = productImageService;
            _fileStorageService = fileStorageService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage([FromForm] UploadProductImageRequest request)
        {
            if (request.Image == null || request.Image.Length == 0)
            {
                return BadRequest(new
                {
                    Message = "Image is required"
                });
            }

            var imageUrl = await _fileStorageService.SaveFileAsync(request.Image.OpenReadStream(), request.Image.FileName, "uploads/products");

            var productImageRequest = new AddProductImageRequestDto
            {
                ProductVariantId = request.ProductVariantId,
                ImageUrl = imageUrl,
                IsPrimary = request.IsPrimary,
                DisplayOrder = request.DisplayOrder
            };

            var result = await _productImageService.AddProductImageAsync(productImageRequest);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("variant-images/{productVariantId}")]
        public async Task<IActionResult> GetImagesByVariantId(Guid productVariantId)
        {
            var result = await _productImageService.GetImagesByVariantIdAsync(productVariantId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
