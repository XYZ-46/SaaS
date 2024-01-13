using DataEntity;

namespace InterfaceProject.Repository
{
    public interface IBaseCrudRepository<T> where T : BaseEntity
    {
        public Task<T?> FindByIdAsync(T TModel);
        public Task<T?> FindByIdAsync(int Id);

        public Task<T> InsertAsync(T TModel);
        public Task<T> UpdateAsync(T TModel);

        public Task<bool> DeleteAsync(T TModel);
    }
}
