using FluentValidation;

namespace Skinet.API.Features.ProductBrands.Commands.Update
{
    public class UpdateProductBrandCommandValidation : AbstractValidator<UpdateProductBrandCommand>
    {
        public UpdateProductBrandCommandValidation()
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
