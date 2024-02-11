namespace DataEntity.Pagination
{
    public class PagingRequest<TModel> where TModel : class
    {

        public int? PageNumber { get; set; } = 1;

        private PageSizeEnum _pageSize { get; set; } = PageSizeEnum.SEPULUH;
        public int PageSize
        {
            get => (int)_pageSize;
            set
            {
                if (Enum.IsDefined(typeof(PageSizeEnum), value)) _pageSize = (PageSizeEnum)value;
                else _pageSize = PageSizeEnum.SEPULUH;
            }
        }

        public List<SearchCriteria<TModel>>? Search { get; set; } = [];
        public List<SortCriteria<TModel>>? Sort { get; set; } = [];
    }

    public class SearchCriteria<TModel> where TModel : class
    {
        public string PropertyName { get; set; }
        public string? PropertyValue { get; set; }
        public string Operator { get; set; }
        public string? PropertyValue1 { get; set; }
        public string? PropertyValue2 { get; set; }
    }
    public class SortCriteria<TModel> where TModel : class
    {
        public const string ORDER_BY_DESCENDING = "desc";

        public bool IsAscending { get; set; } = true;
        public string PropertyNameOrder { get; set; }
    }

    public enum PageSizeEnum
    {
        SEPULUH = 10,
        DUA_PULUH = 20,
        LIMA_PULUH = 50,
        SERATUS = 100
    }
}
