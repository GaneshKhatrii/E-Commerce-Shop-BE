using ECommerce.Application.DTOs.Products;
using FluentValidation;

namespace ECommerce.Application.Validators.Products
{
    public class SearchProductsRequestDtoValidator : AbstractValidator<SearchProductsRequestDto>
    {
        public SearchProductsRequestDtoValidator()
        {
            RuleFor(x => x.MinPrice)
                .GreaterThanOrEqualTo(0)
                .WithMessage("MinPrice cannot be negative.")
                .When(x => x.MinPrice.HasValue);

            RuleFor(x => x.MaxPrice)
                .GreaterThanOrEqualTo(0)
                .WithMessage("MaxPrice cannot be negative.")
                .When(x => x.MaxPrice.HasValue);

            RuleFor(x => x)
                .Must(x =>
                    !x.MinPrice.HasValue ||
                    !x.MaxPrice.HasValue ||
                    x.MinPrice <= x.MaxPrice)
                .WithMessage("Min price cannot be greater than max price");

            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("Page number should be greater than 0");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("Page size must be between 1 to 100 only");
        }
    }
}
