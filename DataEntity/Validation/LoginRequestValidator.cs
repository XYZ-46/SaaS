using DataEntity.Request;
using FluentValidation;

namespace DataEntity.Validation
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Username).NotNull().NotEmpty().WithMessage("{PropertyName} is required, can not empty");
            RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("{PropertyName} is required, can not empty");
        }
    }
}
