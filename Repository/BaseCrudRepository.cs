using DataEntity;
using InterfaceProject;
using Microsoft.EntityFrameworkCore;
using Repository.Database;

namespace Repository
{
    public abstract class BaseCrudRepository<TModel>(AzureDB azureDB) : IBaseCrudRepository<TModel> where TModel : BaseEntity
    {
        public readonly AzureDB _azureDB = azureDB;

        public async Task<TModel?> FindByIdAsync(TModel Tmodel)
        {
            TModel? existingTModel = await _azureDB.Set<TModel>().SingleOrDefaultAsync(x => x.Id.Equals(Tmodel.Id));
            return existingTModel;
        }

        public async Task<TModel?> FindByIdAsync(int Id)
        {
            TModel? existingTModel = await _azureDB.Set<TModel>().SingleOrDefaultAsync(x => x.Id.Equals(Id));
            return existingTModel;
        }

        public async Task<TModel> InsertAsync(TModel Tmodel)
        {
            _azureDB.Set<TModel>().Add(Tmodel);
            await _azureDB.SaveChangesAsync();
            return Tmodel;
        }

        public async Task<TModel> UpdateAsync(TModel Tmodel)
        {
            _ = await FindByIdAsync(Tmodel.Id) ?? throw new DbUpdateException("No Data Found For Updated");

            _azureDB.Set<TModel>().Update(Tmodel);
            await _azureDB.SaveChangesAsync();
            return Tmodel;
        }

        public async Task<bool> DeleteAsync(TModel Tmodel)
        {
            bool result = false;

            TModel? existingTModel = await FindByIdAsync(Tmodel.Id);
            if (existingTModel != null)
            {
                Tmodel.IsDelete = true;
                await UpdateAsync(Tmodel);
                result = true;
            }

            return result;
        }

        public virtual IQueryable<TModel> BaseQuery(int rowSize) => _azureDB.Set<TModel>().Take(rowSize).AsQueryable();
    }
}
