using ECommerce.Application.DTOs.Orders;
using FluentValidation;

namespace ECommerce.Application.Validators.Orders
{
    public class AddCartItemRequestDtoValidator : AbstractValidator<AddCartItemRequestDto>
    {
        public AddCartItemRequestDtoValidator()
        {
            RuleFor(x => x.ProductVariantId)
                .NotEmpty()
                .WithMessage("Product variant id is required.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greter than 0.");
        }
    }
}
