namespace API.Pagination
{
    public class PagingRequest
    {
        public int page { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public object[] search { get; set; } = [];
        public object[] sort { get; set; } = [];
    }
}
