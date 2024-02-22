namespace DataEntity.Pagination
{
    public class SearchCriteria
    {
        public string? PropertyName { get; set; } = string.Empty;
        public string? Value { get; set; } = string.Empty;

        public string Operator { get; set; } = string.Empty;
        //    set => Enum.TryParse(value, out _operator);

        public string? StartValue { get; set; } = string.Empty;
        public string? EndValue { get; set; } = string.Empty;
    }
}
