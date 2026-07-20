using ECommerce.Application.DTOs.Admin;
using ECommerce.Application.DTOs.Admin.Products;
using ECommerce.Application.Interfaces.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboardStats()
        {
            var result = await _adminService.GetDashboardStatsAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _adminService.GetAllUsersAsync(pageNumber, pageSize);
            return StatusCode(result.StatusCode, result);
        }

        // SAME METHOD ALREADY EXISTS IN PRODUCT CONTROLLER ASWELL
        [HttpGet("products")]
        public async Task<IActionResult> GetAllProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _adminService.GetAllProductsAsync(pageNumber, pageSize);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetAllOrders([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _adminService.GetAllOrdersAsync(pageNumber, pageSize);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("orders/{orderId}")]
        public async Task<IActionResult> GetOrderDetails([FromRoute] Guid orderId)
        {
            var result = await _adminService.GetOrderDetailsByIdAsync(orderId);
            return StatusCode(result.StatusCode, result);
        }

        // >>>>>>>>>>>>>>>| Order Status Management |Module<<<<<<<<<<<<<<<
        [HttpPatch("orders/{orderId}/status")]
        public async Task<IActionResult> UpdateOrderStatus([FromRoute] Guid orderId, [FromBody] UpdateOrderStatusRequestDto request)
        {
            var result = await _adminService.UpdateOrderStatusAsync(orderId, request);
            return StatusCode(result.StatusCode, result);
        }

        // >>>>>>>>>>>>>>>| Product  Module |<<<<<<<<<<<<<<<

        // ******> THESE METHODS ARE MOVED FROM PRODUCT MODULE TO ADMIN MODULE BECAUSE ONLY ADMIN CAN ADD PRODUCT, CATEGORY AND BRAND

        [HttpPost("categories")]
        public async Task<IActionResult> AddProductCategory(AddProductCategoryRequestDto request)
        {
            var result = await _adminService.AddProductCategoryAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("brands")]
        public async Task<IActionResult> AddBrand(AddBrandRequestDto request)
        {
            var result = await _adminService.AddBrandAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("products")]
        public async Task<IActionResult> AddProduct(AddProductRequestDto request)
        {
            var result = await _adminService.AddProductAsync(request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("products/{productId:guid}")]
        public async Task<IActionResult> GetProductById([FromRoute] Guid productId)
        {
            var result = await _adminService.GetProductByIdAsync(productId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("products/{productId:guid}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] Guid productId, UpdateProductRequestDto request)
        {
            var result = await _adminService.UpdateProductAsync(productId, request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("products/{productId:guid}/status")]
        public async Task<IActionResult> UpdateProductStatus([FromRoute] Guid productId, UpdateProductStatusDto request)
        {
            var result = await _adminService.UpdateProductStatusAsync(productId, request);
            return StatusCode(result.StatusCode, result);
        }
    }
}
