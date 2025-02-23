using FluentValidation;
using Skinet.API.DTOs.Identity;

namespace Skinet.API.Features.Authentication.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(e => e.DisplayName)
                .NotEmpty()
                .WithMessage("Display Name is required.")
                .MinimumLength(3)
                .WithMessage("Display Name must be at least 3 characters long.");

            RuleFor(e => e.PhoneNumber)
                .NotEmpty()
                .WithMessage("Phone Number is required.");
            //.Matches(@"^\+?\d{10,15}$").WithMessage("Invalid phone number format.");

            RuleFor(e => e.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Invalid email format.");

            RuleFor(e => e.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long.");
        }
    }
}
