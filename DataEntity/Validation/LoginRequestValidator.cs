using DataEntity.Request;
using FluentValidation;

namespace DataEntity.Validation
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotNull().NotEmpty()
                .MinimumLength(5);

            RuleFor(x => x.Password).NotNull().NotEmpty();
        }
    }
}
