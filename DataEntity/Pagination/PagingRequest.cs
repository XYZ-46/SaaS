namespace DataEntity.Pagination
{
    public class PagingRequest
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

        //[ValidPropertyValidation]
        public List<SearchCriteria>? Search { get; set; } = [];
        public List<SortCriteria>? Sort { get; set; } = [];
    }

    public class SearchCriteria
    {
        public string PropertyName { get; set; }
        public string? PropertyValue { get; set; }
        public string Operator { get; set; }
        public string? PropertyValue1 { get; set; }
        public string? PropertyValue2 { get; set; }
    }
    public class SortCriteria
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
