namespace API.Pagination
{
    public class PagingRequest
    {

        public int? page { get; set; } = 1;
        public int? pageSize { get; set; } = 10;
        public List<SearchCriteria>? search { get; set; } = [];
        public List<SortCriteria>? sort { get; set; } = [];
    }

    public record SearchCriteria
    {
        public string PropertyName { get; set; }
        public string? PropertyValue { get; set; }
        public string Operator { get; set; }
        public string? PropertyValue1 { get; set; }
        public string? PropertyValue2 { get; set; }
    }
    public record SortCriteria
    {
        public const string ORDER_BY_DESCENDING = "desc";

        public bool IsAscending { get; set; } = true;
        public string PropertyNameOrder { get; set; }

        private string? _orderType;
        public string? OrderType
        {
            get => _orderType;
            set
            {
                _orderType = value;
                IsAscending = !(OrderType ?? "desc").Equals(ORDER_BY_DESCENDING, StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
