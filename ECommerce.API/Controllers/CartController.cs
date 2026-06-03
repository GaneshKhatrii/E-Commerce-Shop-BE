using ECommerce.API.Helpers;
using ECommerce.Application.DTOs.Orders;
using ECommerce.Application.Interfaces.Orders;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // Using lambda operator
        private Guid userId => CurrentUserHelper.GetUserId(User);

        [HttpPost]
        public async Task<IActionResult> AddToCart(AddCartItemRequestDto request)
        {
            var result = await _cartService.AddToCartAsync(userId, request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserCart()
        {
            var result = await _cartService.GetUserCartAsync(userId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{cartItemId}")]
        public async Task<IActionResult> UpdateCartQuantity([FromRoute] Guid cartItemId, UpdateCartItemQuantityRequestDto request)
        {
            var result = await _cartService.UpdateCartItemQuantityAsync(cartItemId, request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{cartItemId}")]
        public async Task<IActionResult> RemoveCartItem([FromRoute] Guid cartItemId)
        {
            var result = await _cartService.RemoveCartItemAsync(cartItemId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
