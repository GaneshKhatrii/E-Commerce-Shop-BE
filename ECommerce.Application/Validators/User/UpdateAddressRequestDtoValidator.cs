using ECommerce.Application.DTOs.User;
using FluentValidation;

namespace ECommerce.Application.Validators.User
{
    public class UpdateAddressRequestDtoValidator : AbstractValidator<UpdateAddressRequestDto>
    {
        public UpdateAddressRequestDtoValidator()
        {

            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage("Full name is required")
                .MaximumLength(100)
                .WithMessage("Full name cannot exceed 100 characters");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .WithMessage("Phone number is required")
                .MaximumLength(20)
                .WithMessage("Phone number cannot exceed 20 characters");

            RuleFor(x => x.AddressLine1)
                .NotEmpty()
                .WithMessage("Address is required")
                .MaximumLength(200)
                .WithMessage("Address cannot exceed 200 characters");

            RuleFor(x => x.City)
                .NotEmpty()
                .WithMessage("City is required")
                .MaximumLength(100)
                .WithMessage("City cannot exceed 100 characters");

            RuleFor(x => x.State)
                .NotEmpty()
                .WithMessage("State is required")
                .MaximumLength(100)
                .WithMessage("State cannot exceed 100 characters");

            RuleFor(x => x.Country)
                .NotEmpty()
                .WithMessage("Country is required")
                .MaximumLength(100)
                .WithMessage("Country cannot exceed 100 characters");

            RuleFor(x => x.PostalCode)
                .NotEmpty()
                .WithMessage("Postal code is required")
                .MaximumLength(20)
                .WithMessage("Postal code cannot exceed 20 characters");

            RuleFor(x => x.AddressType)
                .IsInEnum()
                .WithMessage("Invalid address type");
        }
    }
}
