using FluentValidation;

namespace Skinet.API.Features.ProductTypes.Commands.Create
{
    public class CreateProductTypeCommandValidator : AbstractValidator<CreateProductTypeCommand>
    {
        public CreateProductTypeCommandValidator()
        {
            RuleFor(i => i.Name)
                .NotEmpty()
                .WithMessage("Name is required")
                .MaximumLength(100)
                .WithMessage("Name must not exceed 100 characters");
        }
    }
}
