namespace DataEntity.Pagination
{
    public class SearchCriteria
    {
        public string? PropertyName { get; set; } = string.Empty;
        public string? Value { get; set; } = string.Empty;
        public string? StartValue { get; set; } = string.Empty;
        public string? EndValue { get; set; } = string.Empty;

        public string Operator { get; set; } = string.Empty;
    }

    public class FilterCriteria
    {
        public required string PropertyName { get; set; }
        public required string PropertyValue { get; set; }
        public required OperatorEnm Operator { get; set; }
    }
}
