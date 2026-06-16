using ECommerce.Application.DTOs.Admin;
using FluentValidation;

namespace ECommerce.Application.Validators.Orders
{
    public class UpdateOrderStatusRequestDtoValidator : AbstractValidator<UpdateOrderStatusRequestDto>
    {
        public UpdateOrderStatusRequestDtoValidator()
        {
            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Invalid order status");
        }
    }
}
