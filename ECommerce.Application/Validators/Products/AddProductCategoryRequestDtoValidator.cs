using ECommerce.Application.DTOs.Admin.Products;
using FluentValidation;

namespace ECommerce.Application.Validators.Products
{
    public class AddProductCategoryRequestDtoValidator : AbstractValidator<AddProductCategoryRequestDto>
    {
        public AddProductCategoryRequestDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Category name is required")
                .MaximumLength(100)
                .WithMessage("Category name cannot exceed 100 characters");
        }
    }
}
