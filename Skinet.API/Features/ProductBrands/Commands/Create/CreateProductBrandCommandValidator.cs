using FluentValidation;

namespace Skinet.API.Features.ProductBrands.Commands.Create
{
    public class CreateProductBrandCommandValidator : AbstractValidator<CreateProductBrandCommand>
    {
        public CreateProductBrandCommandValidator()
        {
            RuleFor(i => i.Name)
                .NotEmpty()
                .WithMessage("Name is required")
                .MaximumLength(100)
                .WithMessage("Name must not exceed 100 characters");
        }
    }
}
