using ECommerce.Application.DTOs.Products;
using FluentValidation;

namespace ECommerce.Application.Validators.Products
{
    public class AddProductImageRequestDtoValidator : AbstractValidator<AddProductImageRequestDto>
    {
        public AddProductImageRequestDtoValidator()
        {
            RuleFor(x => x.ProductVariantId)
                .NotEmpty()
                .WithMessage("Product variant id is required");

            RuleFor(x => x.ImageUrl)
                .NotEmpty()
                .WithMessage("Image is required");

            RuleFor(x => x.DisplayOrder)
                .GreaterThan(0)
                .WithMessage("Display order must be greater than 0");
        }
    }
}
