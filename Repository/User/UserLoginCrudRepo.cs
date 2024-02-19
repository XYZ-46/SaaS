using DataEntity.Model;
using InterfaceProject.User;
using Microsoft.EntityFrameworkCore;
using Repository.Database;

namespace Repository.User
{
    public class UserLoginCrudRepo(AzureDB azureDB) : BaseCrudRepository<UserLoginModel>(azureDB), IUserLoginCrudRepo
    {
        //public override IQueryable<UserLoginModel> BaseQuery(int rowSize) => _azureDB.Set<UserLoginModel>().AsQueryable<UserLoginModel>();

        public async Task<UserLoginModel?> FindByUsernameAsync(string username)
        {
            var userLogin = await _azureDB.UserLoginModel.SingleOrDefaultAsync(x => x.Username == username && !x.IsDelete);
            return userLogin;
        }
    }
}
