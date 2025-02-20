using Skinet.API.Features.Orders.Models;

namespace Skinet.API.Features.Orders.Commands.Create
{
    using FluentValidation;

    public class AddressModelValidator : AbstractValidator<AddressModel>
    {
        public AddressModelValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("First name is required")
                .MaximumLength(50)
                .WithMessage("First name cannot exceed 50 characters");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Last name is required")
                .MaximumLength(50)
                .WithMessage("Last name cannot exceed 50 characters");

            RuleFor(x => x.Street)
                .NotEmpty()
                .WithMessage("Street is required")
                .MaximumLength(100)
                .WithMessage("Street cannot exceed 100 characters");

            RuleFor(x => x.City)
                .NotEmpty()
                .WithMessage("City is required")
                .MaximumLength(50)
                .WithMessage("City cannot exceed 50 characters");

            RuleFor(x => x.State)
                .NotEmpty()
                .WithMessage("State is required")
                .MaximumLength(50)
                .WithMessage("State cannot exceed 50 characters");

            RuleFor(x => x.ZipCode)
                .NotEmpty()
                .WithMessage("Zip code is required");

        }
    }

}
