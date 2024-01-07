using DataEntity;

namespace InterfaceProject.Repository
{
    public interface IBaseCrudRepository<T> : IDisposable where T : BaseEntity
    {
        public Task<T?> FindByIdAsync(T TModel);
        public Task<T?> FindByIdAsync(int Id);

        public Task<T> InsertAsync(T TModel);
        public Task<T> Update(T TModel);

        public Task<bool> Delete(T TModel);
    }
}
