using ECommerce.Application.Common;
using ECommerce.Application.DTOs.Orders;
using ECommerce.Application.Interfaces.Orders;
using ECommerce.Domain.Entities.Orders;
using ECommerce.Domain.Entities.Products;

namespace ECommerce.Infrastructure.Services.Orders
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<ApiResponse<string>> AddToCartAsync(Guid userId, AddCartItemRequestDto request)
        {
            // Validate product variant exists
            var productVariant = await _cartRepository.GetProductVariantByIdAsync(request.ProductVariantId);

            if (productVariant == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Product variant not found"
                };
            }

            // Validate inventory exists
            var inventory = await _cartRepository.GetInventoryByVariantIdAsync(request.ProductVariantId);

            if (inventory == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Inventory not found"
                };
            }

            // Validate stock availability
            if (IsOutOfStock(inventory))
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Out of stock"
                };
            }

            // Validate requested quantity
            if (IsRequestQuantityExceedsStock(inventory, request.Quantity))
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = $"Requested quantity exceeds available stock. Available stock is {inventory.AvailableStock}"
                };
            }

            // Get user's cart
            var userCart = await _cartRepository.GetCartByUserIdAsync(userId);

            // Create cart if not exists
            if (userCart == null)
            {
                userCart = new Cart
                {
                    UserId = userId
                };

                await _cartRepository.AddCartAsync(userCart);
                await _cartRepository.SaveChangesAsync();
            }

            // Check if the product variant is already in the cart
            var existingCartItem = await _cartRepository.GetCartItemAsync(userCart.Id, request.ProductVariantId);

            // Product already exists in cart
            if (existingCartItem != null)
            {
                var updatedQuantity = existingCartItem.Quantity + request.Quantity;

                // Validate requested quantity
                if (IsRequestQuantityExceedsStock(inventory, updatedQuantity))
                {
                    return new ApiResponse<string>
                    {
                        Success = false,
                        StatusCode = 400,
                        Message = $"Requested quantity exceeds available stock. Available stock is {inventory.AvailableStock}"
                    };
                }

                // Finally update the cart item quantity if all validations pass
                existingCartItem.Quantity = updatedQuantity;
            }
            else
            {
                // Create new cart item
                var cartItem = new CartItem
                {
                    CartId = userCart.Id,
                    ProductVariantId = request.ProductVariantId,
                    Quantity = request.Quantity
                };

                await _cartRepository.AddCartItemAsync(cartItem);
            }

            await _cartRepository.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Success = true,
                StatusCode = 200,
                Message = "Product added to cart successfully"
            };
        }

        public async Task<ApiResponse<CartResponseDto>> GetUserCartAsync(Guid userId)
        {
            // Get user cart
            var userCart = await _cartRepository.GetCartByUserIdAsync(userId);

            // Return empty response if cart is null
            if (userCart == null)
            {
                return new ApiResponse<CartResponseDto>
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Cart is empty",
                    Data = new CartResponseDto()
                };
            }

            // Map cart items
            // Select() returns IEnumerable<T>, but our "Cart" DTO expects CartItems as a List<T>, so .ToList() is required.
            var cartItems = userCart.CartItems.Select(cartItem => new CartItemResponseDto
            {
                Id = cartItem.Id,
                ProductVariantId = cartItem.ProductVariantId,
                ProductName = cartItem.ProductVariant.Product.Name,
                Size = cartItem.ProductVariant.Size,
                Color = cartItem.ProductVariant.Color,
                // ImageUrl = primary image url OR empty string if no image exists
                ImageUrl = cartItem.ProductVariant.ProductImages.FirstOrDefault(x => x.IsPrimary)?.ImageUrl ?? string.Empty,
                Price = cartItem.ProductVariant.Price,
                Quantity = cartItem.Quantity,
                SubTotal = cartItem.Quantity * cartItem.ProductVariant.Price,
            }).ToList();

            var totalAmount = cartItems.Sum(x => x.SubTotal);

            var response = new CartResponseDto
            {
                Id = userCart.Id,
                CartItems = cartItems,
                TotalAmount = totalAmount
            };

            return new ApiResponse<CartResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Cart retrieved successfully",
                Data = response
            };
        }

        public async Task<ApiResponse<string>> UpdateCartItemQuantityAsync(Guid cartItemId, UpdateCartItemQuantityRequestDto request)
        {
            // Find cart item
            var cartItem = await _cartRepository.GetCartItemByIdAsync(cartItemId);

            // Return empty response if cart is null
            if (cartItem == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Cart item not found",
                };
            }

            // Get inventory
            var inventory = await _cartRepository.GetInventoryByVariantIdAsync(cartItem.ProductVariantId);

            if (inventory == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Inventory not found",
                };
            }

            // Validate stock
            if (IsRequestQuantityExceedsStock(inventory, request.Quantity))
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    StatusCode = 400,
                    Message = $"Requested quantity exceeds available stock. Available stock is {inventory.AvailableStock}"
                };
            }

            // Update quantity
            cartItem.Quantity = request.Quantity;
            await _cartRepository.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Success = true,
                StatusCode = 200,
                Message = "Cart quantity updated successfully"
            };
        }

        public async Task<ApiResponse<string>> RemoveCartItemAsync(Guid cartItemId)
        {
            // Find cart item
            var cartItem = await _cartRepository.GetCartItemByIdAsync(cartItemId);

            // Return empty response if cart is null
            if (cartItem == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Cart item not found",
                };
            }

            // Remove cart item
            _cartRepository.RemoveCartItem(cartItem);

            await _cartRepository.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Success = true,
                StatusCode = 200,
                Message = "Cart item removed successfully."
            };
        }
        private bool IsOutOfStock(Inventory inventory)
        {
            return inventory.AvailableStock <= 0;
        }

        private bool IsRequestQuantityExceedsStock(Inventory inventory, int requestedQuantity)
        {
            return requestedQuantity > inventory.AvailableStock;
        }
    }
}
