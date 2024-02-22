namespace DataEntity.Pagination
{
    public class SearchCriteria
    {
        public string? PropertyName { get; set; } = string.Empty;
        public string? Value { get; set; } = string.Empty;

        private OperatorEnm _operator;
        public OperatorEnm GetOperator() => _operator;
        public void SetOperator() => Enum.TryParse(this.Operator,true, out _operator);

        public string Operator { get; set; } = string.Empty;

        public string? StartValue { get; set; } = string.Empty;
        public string? EndValue { get; set; } = string.Empty;
    }
}
