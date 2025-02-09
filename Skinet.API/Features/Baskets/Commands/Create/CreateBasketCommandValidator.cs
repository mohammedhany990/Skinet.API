using FluentValidation;

namespace Skinet.API.Features.Baskets.Commands.Create
{
    public class CreateBasketCommandValidator : AbstractValidator<CreateBasketCommand>
    {
        public CreateBasketCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Basket ID is required.");

            RuleFor(x => x.Items)
                .NotEmpty()
                .WithMessage("Basket must contain at least one item.");

            RuleForEach(x => x.Items)
                .SetValidator(new BasketItemModelValidator());
        }
    }
}
