namespace DataEntity
{
    public class Paging
    {

        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public List<int> ListPageSize { get; private set; } = [10, 20, 50, 100];

        public int TotalPages { get; private set; } = 0;
        public int MaxViewPages { get; private set; } = 10;

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
    }
}
