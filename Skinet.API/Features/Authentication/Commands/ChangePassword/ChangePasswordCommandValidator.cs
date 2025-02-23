using FluentValidation;
using Skinet.Core.DTOs.Identity;

namespace Skinet.API.Features.Authentication.Commands.ChangePassword
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Invalid email format.");

            RuleFor(x => x.CurrentPassword)
                .NotEmpty()
                .WithMessage("Current password is required.");

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .WithMessage("New password is required.")
                .MinimumLength(6)
                .WithMessage("New password must be at least 6 characters long.");
            //.Matches(@"[A-Z]").WithMessage("New password must contain at least one uppercase letter.")
            //.Matches(@"[a-z]").WithMessage("New password must contain at least one lowercase letter.")
            //.Matches(@"\d").WithMessage("New password must contain at least one digit.")
            //.Matches(@"[\W]").WithMessage("New password must contain at least one special character.");
        }
    }

}
