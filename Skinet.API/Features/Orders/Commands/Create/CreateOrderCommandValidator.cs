using FluentValidation;
using Skinet.API.Features.Users.Queries;

namespace Skinet.API.Features.Orders.Commands.Create
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            //RuleFor(x => x.BasketId)
            //    .NotEmpty()
            //    .WithMessage("Basket ID is required");

            RuleFor(x => x.DeliveryMethodId)
                .GreaterThan(0)
                .WithMessage("Delivery method must be a valid ID");

            RuleFor(x => x.ShippingAddress)
                .NotNull()
                .WithMessage("Shipping address is required")
                .SetValidator(new AddressModelValidator());
        }
    }

}
