using ECommerce.Application.DTOs.Orders;
using FluentValidation;

namespace ECommerce.Application.Validators.Orders
{
    public class UpdateCartItemQuantityRequestDtoValidator : AbstractValidator<UpdateCartItemQuantityRequestDto>
    {
        public UpdateCartItemQuantityRequestDtoValidator()
        {
            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0.");
        }
    }
}
