using DataEntity;
using DataEntity.Model;
using DataEntity.Pagination;
using InterfaceProject;
using Microsoft.EntityFrameworkCore;
using Repository.Database;
using System.Linq.Expressions;

namespace Repository
{
    public abstract class BaseRepository<TModel>(AzureDB azureDB) : IBaseRepository<TModel>
        where TModel : BaseEntity
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

        public virtual IQueryable<TModel> BaseQuery() => _azureDB.Set<TModel>().AsQueryable();

        public virtual IQueryable<TModel> SearchQuery(List<SearchCriteria> search)
        {
            return _azureDB.Set<TModel>().AsQueryable<TModel>();
        }

        public virtual IQueryable<UserProfileModel> SortQuery(List<SortCriteria> sort)
        {
            return _azureDB.Set<UserProfileModel>().AsQueryable<UserProfileModel>();
        }

        IQueryable<TModel> Filter<TModel>(IQueryable<TModel> queryable, Expression<Func<TModel, bool>> predicate) => queryable.Where(predicate);

    }
}
