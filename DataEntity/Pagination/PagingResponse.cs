namespace DataEntity.Pagination
{
    public class PagingResponse<TModel> where TModel : class
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalData { get; private set; }
        public int TotalPages { get; private set; }
        public List<TModel> PageDataList { get; set; } = [];

        public PagingResponse(List<TModel> items, int count, int pageNumber, int pageSize)
        {
            TotalData = count;
            PageSize = pageSize;
            PageIndex = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            PageDataList.AddRange(items);
        }

        public static PagingResponse<TModel> ToPagedList(IQueryable<TModel> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            var asd = new PagingResponse<TModel>(items, count, pageNumber, pageSize);
            return asd;
        }

    }
}