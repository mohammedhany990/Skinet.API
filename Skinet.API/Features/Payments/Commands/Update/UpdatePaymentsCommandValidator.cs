using FluentValidation;

namespace Skinet.API.Features.Payments.Commands.Update
{
    public class UpdatePaymentsCommandValidator : AbstractValidator<UpdatePaymentsCommand>
    {
        public UpdatePaymentsCommandValidator()
        {
            //RuleFor(x => x.BasketId)
            //    .NotEmpty()
            //    .WithMessage("Basket Id is required");
        }
    }
}
