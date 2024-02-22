using DataEntity.Model;
using DataEntity.Pagination;
using InterfaceProject.Service;
using InterfaceProject.User;
using Microsoft.EntityFrameworkCore;
using Repository.Database;

namespace Repository.User
{
    public class UserProfileRepository(AzureDB azureDB, IRedisService cacheHandler)
        : BaseRepository<UserProfileModel>(azureDB), IUserProfileRepository
    {
        private readonly IRedisService _cacheHandler = cacheHandler;

        public async Task<UserProfileModel?> FindByEmailAsync(string email)
        {
            var userProfile = await _azureDB.UserProfileModel
                .Include(x => x.UserLogin)
                .SingleOrDefaultAsync(x => x.Email == email && !x.IsDelete);
            return userProfile;
        }

        public async Task<UserProfileModel?> FindByFullNameAsync(string fullname) => await _azureDB.UserProfileModel.SingleOrDefaultAsync(x => x.Fullname == fullname && !x.IsDelete);

        public async Task<UserProfileModel?> FindByUserLoginUsernameAsync(string username)
        {
            var keyCache = $"UserProfileModel:{username}";
            var cacheData = await _cacheHandler.GetDataAsync<UserProfileModel>(keyCache);
            if (cacheData != null) return cacheData;

            var userProfile = await _azureDB.UserProfileModel
                                            .Include(x => x.UserLogin)
                                            .SingleOrDefaultAsync(x => x.UserLogin.Username == username && !x.IsDelete && !x.UserLogin.IsDelete);
            if (userProfile != null)
            {
                // update cache
                var expirationTime = DateTime.Now.AddHours(24).TimeOfDay;
                await _cacheHandler.SetDataAsync(keyCache, userProfile, expirationTime);
            }

            return userProfile;
        }

        public async Task<UserProfileModel?> FindByUserLoginIdAsync(int userLoginId)
        {
            var userProfile = await _azureDB.UserProfileModel.SingleOrDefaultAsync(x => x.UserLoginId == userLoginId && !x.IsDelete);
            return userProfile;
        }

        public IQueryable<UserProfileModel> PageQuery(PagingRequest pageRequest)
        {
            var query = BaseQuery();
            pageRequest.Sort.ForEach(srt => query = query.OrderByQuery(srt.PropertyNameOrder, srt.IsAscending));

            return query;
        }

        public PagingResponse<UserProfileModel> PageData(PagingRequest pageRequest)
        {
            var query = PageQuery(pageRequest);
            var data = ToPagedList(query, pageRequest.PageIndex, pageRequest.PageSize);

            return data;
        }

        public static PagingResponse<UserProfileModel> ToPagedList(IQueryable<UserProfileModel> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagingResponse<UserProfileModel>(items, count, pageNumber, pageSize);
        }

        public override IQueryable<UserProfileModel> BaseQuery() => _azureDB.Set<UserProfileModel>().AsQueryable<UserProfileModel>();

        public override IQueryable<UserProfileModel> SearchQuery(List<SearchCriteria> search) => _azureDB.Set<UserProfileModel>().AsQueryable<UserProfileModel>();

    }
}
