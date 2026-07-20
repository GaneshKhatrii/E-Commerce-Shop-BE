using ECommerce.Application.DTOs.Products;
using ECommerce.Application.Interfaces.Products;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("get-products")]
        public async Task<IActionResult> GetProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _productService.GetProductsAsync(pageNumber, pageSize);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("get-product/{productId}")]
        public async Task<IActionResult?> GetProductById(Guid productId)
        {
            var result = await _productService.GetProductByIdAsync(productId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("get-categories")]
        public async Task<IActionResult> GetCategories()
        {
            var result = await _productService.GetCategoriesAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("get-brands")]
        public async Task<IActionResult> GetBrands()
        {
            var result = await _productService.GetBrandsAsync();
            return StatusCode(result.StatusCode, result);
        }

        // Product Search & Filtering Module
        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts([FromQuery] SearchProductsRequestDto request)
        {
            var result = await _productService.SearchProductsAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        //**********> BELOW API'S ARE MOVED TO ADMIN MODULE BECAUSE ONLY ADMIN CAN ADD PRODUCTS, CATEGORIES AND BRANDS

        //[Authorize(Roles = "Admin")]
        //[HttpPost("add-product-category")]
        //public async Task<IActionResult> AddProductCategory(AddProductCategoryRequestDto request)
        //{
        //    var result = await _productService.AddProductCategoryAsync(request);
        //    return StatusCode(result.StatusCode, result);
        //}

        //[Authorize(Roles = "Admin")]
        //[HttpPost("add-brand")]
        //public async Task<IActionResult> AddBrand(AddBrandRequestDto request)
        //{
        //    var result = await _productService.AddBrandAsync(request);
        //    return StatusCode(result.StatusCode, result);
        //}

        //[Authorize(Roles = "Admin")]
        //[HttpPost("add-product")]
        //public async Task<IActionResult> AddProduct(AddProductRequestDto request)
        //{
        //    var result = await _productService.AddProductAsync(request);
        //    return StatusCode(result.StatusCode, result);
        //}
    }
}
