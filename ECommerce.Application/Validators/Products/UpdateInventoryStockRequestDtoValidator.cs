using ECommerce.Application.DTOs.Products;
using FluentValidation;

namespace ECommerce.Application.Validators.Products
{
    public class UpdateInventoryStockRequestDtoValidator : AbstractValidator<UpdateInventoryStockRequestDto>
    {
        public UpdateInventoryStockRequestDtoValidator()
        {
            RuleFor(x => x.AvailableStock)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Available stock must be must be greater than or equal to 0");
        }
    }
}
