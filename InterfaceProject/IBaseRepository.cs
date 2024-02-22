using DataEntity;

namespace InterfaceProject
{
    public interface IBaseRepository<TModel> where TModel : BaseEntity
    {
        public Task<TModel?> FindByIdAsync(TModel Tmodel);
        public Task<TModel?> FindByIdAsync(int Id);

        public Task<TModel> InsertAsync(TModel Tmodel);
        public Task<TModel> UpdateAsync(TModel Tmodel);

        public Task<bool> DeleteAsync(TModel Tmodel);

        IQueryable<TModel> BaseQuery();

    }
}
