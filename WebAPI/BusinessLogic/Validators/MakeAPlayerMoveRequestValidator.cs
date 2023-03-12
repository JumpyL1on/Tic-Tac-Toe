using Common.Requests;
using FluentValidation;

namespace BusinessLogic.Validators
{
    public class MakeAPlayerMoveRequestValidator : AbstractValidator<MakeAPlayerMoveRequest>
    {
        public MakeAPlayerMoveRequestValidator()
        {
            RuleFor(request => request.I)
                .InclusiveBetween(0, 2)
                .WithMessage("Index i is out of range");

            RuleFor(request => request.J)
                .InclusiveBetween(0, 2)
                .WithMessage("Index j is out of range");
        }
    }
}