using ECommerce.Application.DTOs.Products;
using FluentValidation;

namespace ECommerce.Application.Validators.Products
{
    public class AddInventoryRequestDtoValidator : AbstractValidator<AddInventoryRequestDto>
    {
        public AddInventoryRequestDtoValidator()
        {
            RuleFor(x => x.ProductVariantId)
                .NotEmpty()
                .WithMessage("Product variant id is required");

            RuleFor(x => x.AvailableStock)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Available stock must be must be greater than or equal to 0");
        }
    }
}
