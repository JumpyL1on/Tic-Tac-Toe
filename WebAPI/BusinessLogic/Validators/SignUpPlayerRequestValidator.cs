using Common.Requests;
using FluentValidation;

namespace BusinessLogic.Validators
{
    public class SignUpPlayerRequestValidator : AbstractValidator<SignUpPlayerRequest>
    {
        public SignUpPlayerRequestValidator()
        {
            RuleFor(request => request.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .MaximumLength(256)
                .WithMessage("Email must be less than 256 symbols")
                .EmailAddress()
                .WithMessage("Email has wrong format");

            RuleFor(request => request.Password)
                .NotEmpty()
                .WithMessage("Password is required")
                .MinimumLength(6)
                .WithMessage("Password must be more than 6 symbols");

            RuleFor(request => request.ReEnteredPassword)
                .Equal(request => request.Password)
                .WithMessage("Password and re-entered password must match");
        }
    }
}