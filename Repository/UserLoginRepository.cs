using DataEntity.User;
using InterfaceProject.Repository;
using Microsoft.EntityFrameworkCore;
using Repository.Database;

namespace Repository
{
    public class UserLoginRepository(AzureDB azureDB) : BaseCrudRepository<UserLoginModel>(azureDB), IUserLoginRepository
    {
        public async Task<UserLoginModel?> FindByIdAsync(int Id)
        {
            var userLogin = await _azureDB.UserLoginModel.SingleOrDefaultAsync(x => x.Id == Id && !x.IsDelete);
            return userLogin;
        }

        public async Task<UserLoginModel?> FindByUsernameAsync(string username)
        {
            var userLogin = await _azureDB.UserLoginModel.SingleOrDefaultAsync(x => x.Username == username && !x.IsDelete);
            return userLogin;
        }
    }
}
