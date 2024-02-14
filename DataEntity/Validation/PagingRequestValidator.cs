using DataEntity.Pagination;
using FluentValidation;

namespace DataEntity.Validation
{
    public class PagingRequestValidator : AbstractValidator<PagingRequest>
    {
        public PagingRequestValidator()
        {
            RuleForEach(x => x.Search).SetValidator(new SearchCriteriaValidator());
            RuleForEach(x => x.Sort).SetValidator(new SortCriteriaValidator());

            RuleFor(x => x.Sort).Must(list => list?.Count <= 2).WithMessage("Maximum number of Sort items is 2");
            RuleFor(x => x.Sort)
                .Must(list => list?.GroupBy(z => z.PropertyNameOrder).ToList().Count == list?.Count)
                .WithMessage("Sort items must be unique");
        }
    }

    public class SearchCriteriaValidator : AbstractValidator<SearchCriteria>
    {
        public SearchCriteriaValidator()
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

    public class SortCriteriaValidator : AbstractValidator<SortCriteria>
    {
        public SortCriteriaValidator() => RuleFor(x => x.PropertyNameOrder).NotNull().NotEmpty();
    }
}
