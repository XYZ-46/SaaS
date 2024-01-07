using DataEntity.User;
using InterfaceProject.Repository;
using Microsoft.EntityFrameworkCore;
using Repository.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UserProfileRepository(AzureDB azureDB) : BaseCrudRepository<UserProfileModel>(azureDB), IUserProfileRepository
    {
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

        public async Task<UserProfileModel?> FindByUserLoginIdAsync(int userLoginId)
        {
            var userProfile = await _azureDB.UserProfileModel.SingleOrDefaultAsync(x => x.UserLoginId == userLoginId && !x.IsDelete);
            return userProfile;
        }
    }
}
