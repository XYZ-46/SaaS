using DataEntity;

namespace InterfaceProject.Repository
{
    public interface IBaseCrudRepository<TModel> where TModel : BaseEntity
    {
        public Task<TModel?> FindByIdAsync(TModel TModel);
        public Task<TModel?> FindByIdAsync(int Id);

        public Task<TModel> InsertAsync(TModel TModel);
        public Task<TModel> UpdateAsync(TModel TModel);

        public Task<bool> DeleteAsync(TModel TModel);
    }
}
