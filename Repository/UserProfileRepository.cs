using DataEntity.User;
using InterfaceProject.Repository;
using Microsoft.EntityFrameworkCore;
using Repository.Database;

namespace Repository
{
    public class UserProfileRepository(AzureDB azureDB) : BaseCrudRepository<UserProfileModel>(azureDB), IUserProfileRepository
    {
        public async Task<UserProfileModel?> FindByEmailAsync(string email)
        {
            var userProfile = await _azureDB.UserProfileModel
                .Include(x => x.UserLogin)
                .SingleOrDefaultAsync(x => x.Email == email && !x.IsDelete);
            return userProfile;
        }

        public async Task<UserProfileModel?> FindByFullNameAsync(string fullname)
        {
            var userProfile = await _azureDB.UserProfileModel.SingleOrDefaultAsync(x => x.Fullname == fullname && !x.IsDelete);
            return userProfile;
        }

        public async Task<UserProfileModel?> FindByUserLoginUsernameAsync(string username)
        {
            var userProfile = await _azureDB.UserProfileModel
                .Include(x => x.UserLogin)
                .SingleOrDefaultAsync(x => x.UserLogin.Username == username && !x.IsDelete && !x.UserLogin.IsDelete);
            return userProfile;
        }

        public async Task<UserProfileModel?> FindByUserLoginIdAsync(int userLoginId)
        {
            var userProfile = await _azureDB.UserProfileModel.SingleOrDefaultAsync(x => x.UserLoginId == userLoginId && !x.IsDelete);
            return userProfile;
        }
    }
}
