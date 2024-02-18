using DataEntity.Pagination;

namespace InterfaceProject.Repository
{
    public interface IBaseQueryRepository
    {
        IQueryable BaseQuery(int rowSize);
    }
}
