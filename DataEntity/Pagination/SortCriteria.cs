namespace DataEntity.Pagination
{
    public class SortCriteria
    {
        public const string ORDER_BY_DESCENDING = "desc";

        public bool IsAscending { get; set; } = true;
        public string? PropertyNameOrder { get; set; } = string.Empty;
    }
}
