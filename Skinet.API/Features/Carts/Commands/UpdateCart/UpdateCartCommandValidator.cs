
using FluentValidation;

namespace Skinet.API.Features.Carts.Commands.Update
{
    public class UpdateCartCommandValidator : AbstractValidator<UpdateCartCommand>
    {
        public UpdateCartCommandValidator()
        {
            RuleFor(x => x.PaymentIntentId)
                .NotEmpty().WithMessage("Payment Intent ID is required.");

            RuleFor(x => x.DeliveryMethodId)
                .NotNull().WithMessage("Delivery Method ID is required.");

            RuleFor(x => x.ShippingPrice)
                .GreaterThanOrEqualTo(0).WithMessage("Shipping price cannot be negative.");
        }
    }

}


