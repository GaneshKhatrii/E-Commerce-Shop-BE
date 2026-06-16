using ECommerce.Domain.Enums;

namespace ECommerce.Application.DTOs.Admin
{
    public class UpdateOrderStatusRequestDto
    {
        public OrderStatus Status { get; set; }
    }
}
