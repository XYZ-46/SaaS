using DataEntity;
using InterfaceProject.Repository;
using Microsoft.EntityFrameworkCore;
using Repository.Database;

namespace Repository
{
    public abstract class BaseCrudRepository<T>(AzureDB azureDB) : IBaseCrudRepository<T> where T : BaseEntity
    {
        public readonly AzureDB _azureDB = azureDB;

        public async Task<T?> FindByIdAsync(T TModel)
        {
            T? existingTModel = await _azureDB.Set<T>().SingleOrDefaultAsync(x => x.Id.Equals(TModel.Id));
            return existingTModel;
        }

        public async Task<T?> FindByIdAsync(int Id)
        {
            T? existingTModel = await _azureDB.Set<T>().SingleOrDefaultAsync(x => x.Id.Equals(Id));
            return existingTModel;
        }

        public async Task<T> InsertAsync(T TModel)
        {
            _azureDB.Set<T>().Add(TModel);
            await _azureDB.SaveChangesAsync();
            return TModel;
        }

        public async Task<T> UpdateAsync(T TModel)
        {
            _ = await FindByIdAsync(TModel.Id) ?? throw new DbUpdateException("No Data Found For Updated");

            _azureDB.Set<T>().Update(TModel);
            await _azureDB.SaveChangesAsync();
            return TModel;
        }

        public async Task<bool> DeleteAsync(T TModel)
        {
            bool result = false;

            T? existingTModel = await FindByIdAsync(TModel.Id);
            if (existingTModel != null)
            {
                TModel.IsDelete = true;
                await UpdateAsync(TModel);
                result = true;
            }

            return result;
        }

        public IQueryable<T> GetQueryable() => _azureDB.Set<T>().AsQueryable();
    }
}
