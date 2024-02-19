using InterfaceProject;
using Repository.Database;

namespace Repository
{
    public class BaseQueryRepository<TModel>(AzureDB azureDB) : IBaseQueryRepository where TModel : class
    {
        public readonly AzureDB _azureDB = azureDB;

        public virtual IQueryable BaseQuery(int rowSize)
        {
            return _azureDB.Set<TModel>().Take(rowSize);
        }
    }
}
