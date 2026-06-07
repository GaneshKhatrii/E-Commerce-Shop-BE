using ECommerce.Application.Common;
using ECommerce.Application.DTOs.Orders;

namespace ECommerce.Application.Interfaces.Orders
{
    public interface IOrderService
    {
        Task<ApiResponse<string>> PlaceOrderAsync(Guid userId, PlaceOrderRequestDto request);
        Task<ApiResponse<OrderResponseDto?>> GetOrderByIdAsync(Guid orderId);
        Task<ApiResponse<List<OrderResponseDto>>> GetUserOrdersAsync(Guid userId);

        // Order Status Management Module
        Task<ApiResponse<string>> UpdateOrderStatusAsync(Guid orderId, UpdateOrderStatusRequestDto request);
    }
}
