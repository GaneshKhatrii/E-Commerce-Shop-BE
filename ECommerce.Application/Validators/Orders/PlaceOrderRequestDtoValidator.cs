using ECommerce.Application.DTOs.Orders;
using FluentValidation;

namespace ECommerce.Application.Validators.Orders
{
    public class PlaceOrderRequestDtoValidator : AbstractValidator<PlaceOrderRequestDto>
    {
        public PlaceOrderRequestDtoValidator()
        {
            RuleFor(x => x.AddressId)
                .NotEmpty()
                .WithMessage("Address id is required");
        }
    }
}
