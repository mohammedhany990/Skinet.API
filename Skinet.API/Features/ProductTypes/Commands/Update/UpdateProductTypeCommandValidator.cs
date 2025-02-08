using FluentValidation;

namespace Skinet.API.Features.ProductTypes.Commands.Update
{
    public class UpdateProductTypeCommandValidator : AbstractValidator<UpdateProductTypeCommand>
    {
        public UpdateProductTypeCommandValidator()
        {
            RuleFor(i => i.Id)
                .NotNull()
                .GreaterThan(0)
                .WithMessage("Enter a valid Id");


            RuleFor(i => i.Name)
                .NotEmpty()
                .WithMessage("Name is required")
                .MaximumLength(100)
                .WithMessage("Name must not exceed 100 characters");
        }
    }
}
