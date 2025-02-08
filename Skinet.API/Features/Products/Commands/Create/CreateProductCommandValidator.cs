using FluentValidation;

namespace Skinet.API.Features.Products.Commands.Create
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Product name is required.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required.")
                .MinimumLength(10)
                .WithMessage("Description must be at least 10 characters long.");

            RuleFor(x => x.PictureUrl)
                .NotNull()
                .WithMessage("Product image is required.");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Price must be greater than zero.");

            RuleFor(x => x.ProductBrandId)
                .GreaterThan(0)
                .WithMessage("Invalid Brand ID.");

            RuleFor(x => x.ProductTypeId)
                .GreaterThan(0)
                .WithMessage("Invalid Type ID.");
        }
    }
}
