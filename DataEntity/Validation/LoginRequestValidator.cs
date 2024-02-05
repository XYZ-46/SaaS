using DataEntity.Request;
using FluentValidation;

namespace DataEntity.Validation
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotNull().NotEmpty().WithMessage("{PropertyName} gak boleh kosong")
                .MinimumLength(5).WithMessage("kependekan");
            RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("{PropertyName} is required, can not empty");
        }
    }
}
