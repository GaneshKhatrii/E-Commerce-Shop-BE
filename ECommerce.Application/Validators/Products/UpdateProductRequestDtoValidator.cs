using ECommerce.Application.DTOs.Admin.Products;
using FluentValidation;

namespace ECommerce.Application.Validators.Products
{
    public class UpdateProductRequestDtoValidator : AbstractValidator<UpdateProductRequestDto>
    {
        public UpdateProductRequestDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Product name is required")
                .MaximumLength(200)
                .WithMessage("Product name cannot exceed 200 characters");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Product description cannot is required")
                .MaximumLength(2000)
                .WithMessage("Product description cannot exceed 2000 characters");

            RuleFor(x => x.ProductCategoryId)
                .NotEmpty()
                .WithMessage("Product category is required");

            RuleFor(x => x.BrandId)
                .NotEmpty()
                .WithMessage("Product brand is required");
        }
    }
}
