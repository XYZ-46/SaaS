namespace API.Pagination
{
    public class PagingRequest
    {

        public int page { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public List<SearchCriteria> search { get; set; } = [];
        public List<SortCriteria> sort { get; set; } = [];
    }

    public record SearchCriteria
    {
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }
        public string Operator { get; set; }
    }
    public record SortCriteria
    {
        public const string ORDER_BY_ASCENDING = "asc";

        public string PropertyNameOrder { get; set; }
        public string OrderType { get; set; }
        public bool IsAscending() => OrderType.Equals(ORDER_BY_ASCENDING, StringComparison.OrdinalIgnoreCase);
    }
}
