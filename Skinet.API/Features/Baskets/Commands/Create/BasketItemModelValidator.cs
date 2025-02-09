using FluentValidation;

namespace Skinet.API.Features.Baskets.Commands.Create
{
    public class BasketItemModelValidator : AbstractValidator<BasketItemModel>
    {
        public BasketItemModelValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.ProductName)
                .NotEmpty();

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Price must be greater than 0");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be at least 1");

            RuleFor(x => x.PictureUrl)
                .NotEmpty();

            RuleFor(x => x.Brand)
                .NotEmpty();

            RuleFor(x => x.Type)
                .NotEmpty();
        }
    }
}
