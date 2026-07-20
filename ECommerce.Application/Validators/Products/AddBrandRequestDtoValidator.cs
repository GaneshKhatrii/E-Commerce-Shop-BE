using ECommerce.Application.DTOs.Admin.Products;
using FluentValidation;

namespace ECommerce.Application.Validators.Products
{
    public class AddBrandRequestDtoValidator : AbstractValidator<AddBrandRequestDto>
    {
        public AddBrandRequestDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Brand name is required")
                .MaximumLength(100)
                .WithMessage("Brand name cannot exceed 100 characters");
        }
    }
}
