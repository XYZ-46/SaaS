using DataEntity.User;
using InterfaceProject.Search;
using Microsoft.EntityFrameworkCore;
using Repository.Database;

namespace Search
{
    public class UserProfileSearch(AzureDB azureDB) : IUserProfileSearch
    {
        private readonly AzureDB _azureDB = azureDB;

        public async Task<UserProfileModel?> FindByEmailAsync(string email)
        {
            var userProfile = await _azureDB.UserProfileModel.SingleOrDefaultAsync(x => x.Email == email && !x.IsDelete);
            return userProfile;
        }

        public async Task<UserProfileModel?> FindByFullNameAsync(string fullname)
        {
            var userProfile = await _azureDB.UserProfileModel.SingleOrDefaultAsync(x => x.Fullname == fullname && !x.IsDelete);
            return userProfile;
        }

        public async Task<UserProfileModel?> FindByIDAsync(int id)
        {
            var userProfile = await _azureDB.UserProfileModel.SingleOrDefaultAsync(x => x.Id == id && !x.IsDelete);
            return userProfile;
        }

        public async Task<UserProfileModel?> FindByUserLoginIdAsync(int userLoginId)
        {
            var userProfile = await _azureDB.UserProfileModel.SingleOrDefaultAsync(x => x.UserLoginId == userLoginId && !x.IsDelete);
            return userProfile;
        }
    }
}
