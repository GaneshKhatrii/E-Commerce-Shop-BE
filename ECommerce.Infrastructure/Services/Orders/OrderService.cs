using ECommerce.Application.Common;
using ECommerce.Application.DTOs.Orders;
using ECommerce.Application.Interfaces.Orders;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Orders;
using ECommerce.Domain.Enums;
using System.Numerics;

namespace ECommerce.Infrastructure.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<ApiResponse<string>> PlaceOrderAsync(Guid userId, PlaceOrderRequestDto request)
        {
            // Validate that the selected address exists and belongs to the current user
            var address = await _orderRepository.GetAddressByIdAsync(request.AddressId, userId);

            if (address == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Address not found"
                };
            }

            // Get user's cart along with cart items and product details
            var cart = await _orderRepository.GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Cart not found"
                };
            }

            // Order cannot be placed if cart has no items
            if (!cart.CartItems.Any())
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Cart is empty"
                };
            }

            decimal totalAmount = 0;
            var orderItems = new List<OrderItem>();

            // Loop through each cart item and append each cart item into orderItems list
            foreach (var cartItem in cart.CartItems)
            {
                // Verify inventory record exists for the variant(product item)
                var inventory = await _orderRepository.GetInventoryByVariantIdAsync(cartItem.ProductVariantId);

                if (inventory == null)
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        StatusCode = 404,
                        Message = "Inventory not found"
                    };
                }

                // Prevent ordering more quantity than available stock
                if (inventory.AvailableStock < cartItem.Quantity)
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        StatusCode = 400,
                        Message = $"{cartItem.ProductVariant.Product.Name} is out of stock"
                    };
                }

                // Sum each cart item price to get final total amount
                totalAmount += cartItem.ProductVariant.Price * cartItem.Quantity;

                // Append orderItems list with each cart item one by one
                // Create order item snapshot
                // Price and image are stored so future product changes
                // do not affect already placed orders
                orderItems.Add(new OrderItem
                {
                    ProductVariantId = cartItem.ProductVariantId,
                    Quantity = cartItem.Quantity,
                    UnitPrice = cartItem.ProductVariant.Price,
                    ImageUrl = cartItem.ProductVariant.ProductImages.FirstOrDefault(x => x.IsPrimary)?.ImageUrl ?? string.Empty
                });

                // Reduce Inventory
                inventory.AvailableStock -= cartItem.Quantity;
            }

            // Create order with all the available fetched data
            var order = new Order
            {
                UserId = userId,
                TotalAmount = totalAmount,
                Status = OrderStatus.Pending,

                // Shipping Snapshot
                FullName = address.FullName,
                PhoneNumber = address.PhoneNumber,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                City = address.City,
                State = address.State,
                Country = address.Country,
                PostalCode = address.PostalCode,

                // Include constructed orderItems, if not product details will not shown
                OrderItems = orderItems
            };

            // Add order with all related order items
            await _orderRepository.AddOrderAsync(order);

            // Cart is cleared after successful order creation
            cart.CartItems.Clear();

            // Persist Order, OrderItems, Inventory changes and Cart cleanup
            await _orderRepository.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Success = true,
                StatusCode = 201,
                Message = "Order placed successfully"
            };
        }

        public async Task<ApiResponse<OrderResponseDto?>> GetOrderByIdAsync(Guid orderId)
        {
            // Retrieve complete order details including order items
            var order = await _orderRepository.GetOrderByIdAsync(orderId);

            if (order == null)
            {
                return new ApiResponse<OrderResponseDto?>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Order not found"
                };
            }

            // Map domain entity to response DTO for API response
            var orderData = new OrderResponseDto
            {
                OrderId = orderId,
                TotalAmount = order.TotalAmount,
                Status = order.Status.ToString(),
                CreatedAt = order.CreatedAt,
                OrderItems = order.OrderItems.Select(item => new OrderItemResponseDto
                {
                    ProductVariantId = item.ProductVariantId,
                    ProductName = item.ProductVariant.Product.Name,
                    Size = item.ProductVariant.Size,
                    Color = item.ProductVariant.Color,
                    Quantity = item.Quantity,
                    ImageUrl = item.ImageUrl,
                    UnitPrice = item.UnitPrice,
                    SubTotal = item.UnitPrice * item.Quantity
                }).ToList()
            };

            return new ApiResponse<OrderResponseDto?>
            {
                Success = true,
                StatusCode = 200,
                Message = "Order retrieved successfully.",
                Data = orderData
            };
        }

        public async Task<ApiResponse<List<OrderResponseDto>>> GetUserOrdersAsync(Guid userId)
        {
            // Retrieve all orders placed by the current user
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);

            // Convert domain entities into API response DTOs
            var ordersList = orders.Select(order => new OrderResponseDto
            {
                OrderId = order.Id,
                TotalAmount = order.TotalAmount,
                Status = order.Status.ToString(),
                CreatedAt = order.CreatedAt,
                OrderItems = order.OrderItems.Select(orderItem => new OrderItemResponseDto
                {
                    ProductVariantId = orderItem.ProductVariantId,
                    ProductName = orderItem.ProductVariant.Product.Name,
                    Size = orderItem.ProductVariant.Size,
                    Color = orderItem.ProductVariant.Color,
                    Quantity = orderItem.Quantity,
                    ImageUrl = orderItem.ImageUrl,
                    UnitPrice = orderItem.UnitPrice,
                    SubTotal = orderItem.UnitPrice * orderItem.Quantity
                }).ToList()
            }).ToList();

            return new ApiResponse<List<OrderResponseDto>>
            {
                Success = true,
                StatusCode = 200,
                Message = ordersList.Any() ? "Orders retrieved successfully." : "No orders found.",
                Data = ordersList
            };
        }

        // Order Status Management Module
        public async Task<ApiResponse<string>> UpdateOrderStatusAsync(Guid orderId, UpdateOrderStatusRequestDto request)
        {
            var order = await _orderRepository.GetOrderByIdForUpdateAsync(orderId);

            if (order == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Order not found"
                };
            }

            // Validate allowed status transitions
            var IsValidTransition =
                (order.Status == OrderStatus.Pending &&
                (request.Status == OrderStatus.Confirmed || request.Status == OrderStatus.Cancelled))

                ||  

                (order.Status == OrderStatus.Confirmed &&
                (request.Status == OrderStatus.Shipped || request.Status == OrderStatus.Delivered));

            // Update order status
            order.Status = request.Status;

            // Save Changes
            await _orderRepository.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Success = false,
                StatusCode = 200,
                Message = "Order status changed"
            };
        }
    }
}
