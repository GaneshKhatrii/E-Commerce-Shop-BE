using ECommerce.Application.DTOs.Products;
using FluentValidation;

namespace ECommerce.Application.Validators.Products
{
    public class AddProductVariantRequestDtoValidator : AbstractValidator<AddProductVariantRequestDto>
    {
        public AddProductVariantRequestDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("Product id is required");

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
