using ECommerce.Domain.Enums;

namespace ECommerce.Application.DTOs.Orders
{
    public class UpdateOrderStatusRequestDto
    {
        public OrderStatus Status { get; set; }
    }
}
