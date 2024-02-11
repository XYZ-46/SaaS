using DataEntity.Model;
using DataEntity.Pagination;
using FluentValidation;

namespace DataEntity.Validation
{
    public class PagingUserRequestValidator : AbstractValidator<PagingRequest<UserProfileModel>>
    {

        public PagingUserRequestValidator()
        {

            RuleForEach(x => x.Search).SetValidator(new SearchUserRequestValidator());
            RuleForEach(x => x.Sort).SetValidator(new SortUserRequestValidator());

            RuleFor(x => x.Sort).Must(list => list?.Count <= 2).WithMessage("Maximum number of Sort items is 2");
            //.Must(ls => ls?.GroupBy(z => z.PropertyNameOrder).ToList().Count == ls?.Count);
            RuleFor(x => x.Sort)
                .Must(list => list?.GroupBy(z => z.PropertyNameOrder).ToList().Count == list?.Count)
                .WithMessage("Sort items must be unique");
        }
    }

    public class SearchUserRequestValidator : AbstractValidator<SearchCriteria<UserProfileModel>>
    {
        public SearchUserRequestValidator()
        {

            RuleFor(x => x.PropertyName).NotNull().NotEmpty();

            When(x => !x.Operator.Equals("between", StringComparison.OrdinalIgnoreCase), () =>
            {
                RuleFor(x => x.PropertyValue).NotNull().NotEmpty();
            });

            When(x => x.Operator.Equals("between", StringComparison.OrdinalIgnoreCase), () =>
            {
                RuleFor(x => x.PropertyValue1).NotNull().NotEmpty();
                RuleFor(x => x.PropertyValue2).NotNull().NotEmpty();
            });
        }

    }

    public class SortUserRequestValidator : AbstractValidator<SortCriteria<UserProfileModel>>
    {
        public SortUserRequestValidator()
        {
            RuleFor(x => x.PropertyNameOrder).NotNull().NotEmpty();
        }
    }
}
