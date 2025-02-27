using FluentValidation;
using Skinet.API.Features.Carts.Responses;

namespace Skinet.API.Features.Carts.Responses
{

    public class CartItemResponseValidator : AbstractValidator<CartItemResponse>
    {
        public CartItemResponseValidator()
        {
            RuleFor(x => x.ProductName)
                .NotEmpty().WithMessage("Product name is required.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
        }
    }
}
