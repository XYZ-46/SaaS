namespace InterfaceProject
{
    public interface IBaseQueryRepository
    {
        IQueryable BaseQuery(int rowSize);
    }
}
