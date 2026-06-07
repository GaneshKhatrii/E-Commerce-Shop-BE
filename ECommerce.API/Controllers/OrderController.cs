using ECommerce.API.Helpers;
using ECommerce.Application.DTOs.Orders;
using ECommerce.Application.Interfaces.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        private Guid userId => CurrentUserHelper.GetUserId(User);

        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderRequestDto request)
        {
            var result = await _orderService.PlaceOrderAsync(userId, request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById([FromRoute] Guid orderId)
        {
            var result = await _orderService.GetOrderByIdAsync(orderId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("my-orders")]
        public async Task<IActionResult> GetMyOrders()
        {
            var result = await _orderService.GetUserOrdersAsync(userId);
            return StatusCode(result.StatusCode, result);
        }

        // Order Status Management Module
        [Authorize(Roles = "Admin")]
        [HttpPatch("status/{orderId}")]
        public async Task<IActionResult> UpdateOrderStatus([FromRoute] Guid orderId, [FromBody] UpdateOrderStatusRequestDto request)
        {
            var result = await _orderService.UpdateOrderStatusAsync(orderId, request);
            return StatusCode(result.StatusCode, result);
        }
    }
}
