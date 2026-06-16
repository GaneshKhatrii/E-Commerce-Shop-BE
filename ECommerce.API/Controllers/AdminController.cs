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
    }
}
