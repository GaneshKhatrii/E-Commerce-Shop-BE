using ECommerce.Application.DTOs.Products;
using ECommerce.Application.Interfaces.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductVariantController : ControllerBase
    {
        private readonly IproductVariantService _productVariantService;
        public ProductVariantController(IproductVariantService productVariantService)
        {
            _productVariantService = productVariantService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add-product-variant")]
        public async Task<IActionResult> AddProductVariantAsync(AddProductVariantRequestDto request)
        {
            var result = await _productVariantService.AddProductVariantAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("get-product-variants")]
        public async Task<IActionResult> GetProductVariantsAsync([FromQuery] int pageNumber = 1 , [FromQuery] int pageSize = 10)
        {
            var result = await _productVariantService.GetProductVariantsAsync(pageNumber, pageSize);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("get-variants-byProductId/{productId}")]
        public async Task<IActionResult> GetVariantsByProductIdAsync(Guid productId)
        {
            var result = await _productVariantService.GetVariantsByProductIdAsync(productId);
            return StatusCode(result.StatusCode, result);
        }

    }
}
