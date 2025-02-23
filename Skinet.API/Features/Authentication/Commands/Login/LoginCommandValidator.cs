using FluentValidation;
using Skinet.API.DTOs.Identity;

namespace Skinet.API.Features.Authentication.Commands.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(e => e.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Invalid email format.");

            RuleFor(p => p.Password)
                .NotEmpty()
                .WithMessage("Password is required.");
        }

    }
}
