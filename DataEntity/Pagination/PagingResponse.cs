namespace DataEntity.Pagination
{
    public class PagingResponse<TModel> where TModel : class
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalData { get; private set; }
        public int TotalPages { get; private set; }
        public List<TModel> DataList { get; set; } = [];

        public PagingResponse(List<TModel> items, int count, int pageNumber, int pageSize)
        {
            TotalData = count;
            PageSize = pageSize;
            PageIndex = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            DataList.AddRange(items);
        }
    }
}