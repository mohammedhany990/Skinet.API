using FluentValidation;

namespace Skinet.API.Features.Payments.Commands.Webhook
{
    public class WebhookCommandValidator : AbstractValidator<WebhookCommand>
    {
        public WebhookCommandValidator()
        {
            RuleFor(x => x.Json)
                .NotEmpty().WithMessage("Json payload is required.");

            RuleFor(x => x.StripeSignature)
                .NotEmpty().WithMessage("Stripe Signature is required.");
        }
    }
}
