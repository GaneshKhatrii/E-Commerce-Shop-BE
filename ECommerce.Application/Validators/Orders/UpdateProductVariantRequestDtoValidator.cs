using ECommerce.Application.DTOs.Admin.Products;
using FluentValidation;

namespace ECommerce.Application.Validators.Orders
{
    public class UpdateProductVariantRequestDtoValidator : AbstractValidator<UpdateProductVariantRequestDto>
    {
        public UpdateProductVariantRequestDtoValidator()
        {
            RuleFor(x => x.Size)
                .NotEmpty()
                .WithMessage("Size is required")
                .MaximumLength(20)
                .WithMessage("Size cannot exceed 20 characters");

            RuleFor(x => x.Color)
                .NotEmpty()
                .WithMessage("Color is required")
                .MaximumLength(50)
                .WithMessage("Color cannot exceed 50 characters");

            RuleFor(x => x.Price)
                .NotEmpty()
                .WithMessage("Price is required")
                .GreaterThan(0)
                .WithMessage("Price must be greater than 0");
        }
    }
}
