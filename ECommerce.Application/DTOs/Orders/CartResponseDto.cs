namespace ECommerce.Application.DTOs.Orders
{
    public class CartResponseDto
    {
        // Cart Id
        public Guid? Id { get; set; }

        // new() = new List<CartItemResponseDto>() only... this is new syntax
        public List<CartItemResponseDto> CartItems { get; set; } = new();

        // Sum of all cart items(sub total) in the cart
        public decimal TotalAmount { get; set; }
    }
}
