namespace DataEntity.Pagination
{
    public class PaginatedDataList<TModel>(PagingRequest pageRequest) where TModel : class
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalData { get; private set; }
        public List<TModel> Data { get; set; }

        private readonly PagingRequest _pageRequest = pageRequest;
    }
}