using DataEntity.Pagination;

namespace DataEntity.Mapper
{
    public static class SearchMapper
    {
        public static FilterCriteria MapToFilterCriteria(this SearchCriteria search)
        {
            return new FilterCriteria()
            {
                PropertyName = search.PropertyName,
                PropertyValue = search.Value,
                Operator = (OperatorEnm)Enum.Parse(typeof(OperatorEnm), search.Operator, true)
            };
        }

        public static (FilterCriteria, FilterCriteria) MapToFilterBetweenCriteria(this SearchCriteria search)
        {
            if (!search.Operator.Equals("between", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException($"Invalid operator {search.Operator} MapToFilterBetweenCriteria");

            var startFilter = new FilterCriteria()
            {
                PropertyName = search.PropertyName,
                PropertyValue = search.StartValue,
                Operator = OperatorEnm.GreaterThanOrEqual
            };

            var endFilter = new FilterCriteria()
            {
                PropertyName = search.PropertyName,
                PropertyValue = search.EndValue,
                Operator = OperatorEnm.LessThanOrEqual
            };

            return (startFilter, endFilter);
        }
    }
}
