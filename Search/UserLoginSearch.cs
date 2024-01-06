using DataEntity.User;
using InterfaceProject.Search;
using Microsoft.EntityFrameworkCore;
using Repository.Database;

namespace Search
{
    public class UserLoginSearch(AzureDB azureDB) : IUserLoginSearch
    {
        private readonly AzureDB _azureDB = azureDB;

        public async Task<UserLoginModel?> FindByIdAsync(int Id)
        {
            var userLogin = await _azureDB.UserLoginModel.AsNoTracking().SingleOrDefaultAsync(x => x.Id == Id && !x.IsDelete);
            return userLogin;
        }

        public async Task<UserLoginModel?> FindByUsernameAsync(string username)
        {
            var userLogin = await _azureDB.UserLoginModel.SingleOrDefaultAsync(x => x.Username == username && !x.IsDelete);
            return userLogin;
        }
    }
}
