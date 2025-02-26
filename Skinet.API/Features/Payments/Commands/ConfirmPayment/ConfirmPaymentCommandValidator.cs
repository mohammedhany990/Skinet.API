using FluentValidation;

namespace Skinet.API.Features.Payments.Commands.ConfirmPayment
{
    public class ConfirmPaymentCommandValidator : AbstractValidator<ConfirmPaymentCommand>
    {
        public ConfirmPaymentCommandValidator()
        {
            RuleFor(x => x.PaymentIntentId)
                .NotEmpty()
                .WithMessage("PaymentIntentId is required.");
        }
    }
}
