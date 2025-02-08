using FluentValidation;

namespace Skinet.API.Features.Products.Commands.Update
{
    public class UpdateProductCommandValidator:AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Product Id is required and must be a positive value.");

            RuleFor(x => x.Description)
                .Length(10, int.MaxValue)
                .When(x => x.Description != null) 
                .WithMessage("Description must be at least 10 characters long.");


            RuleFor(x => x.Price)
                .GreaterThan(0)
                .When(x=> x.Price != null)
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
