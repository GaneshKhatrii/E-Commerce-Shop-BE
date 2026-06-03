using ECommerce.Application.Common;
using ECommerce.Application.DTOs.Orders;

namespace ECommerce.Application.Interfaces.Orders
{
    public interface ICartService
    {
        // Add product into cart
        Task<ApiResponse<string>> AddToCartAsync(Guid userId, AddCartItemRequestDto request);

        // Get full user cart
        Task<ApiResponse<CartResponseDto>> GetUserCartAsync(Guid userId);

        // Update cart quantity
        Task<ApiResponse<string>> UpdateCartItemQuantityAsync(Guid cartItemId, UpdateCartItemQuantityRequestDto request);

        // Remove cart item
        Task<ApiResponse<string>> RemoveCartItemAsync(Guid cartItemId);
    }
}
