using DataEntity.User;
using InterfaceProject.Repository;
using Repository.Database;

namespace Repository
{
    public class UserLoginRepo(AzureDB azureDB) : IUserLoginRepo
    {
        private readonly AzureDB _azureDB = azureDB;

        public async Task<UserLoginModel> Insert(UserLoginModel userLoginModel)
        {
            _azureDB.UserLoginModel.Add(userLoginModel);
            await _azureDB.SaveChangesAsync();
            return userLoginModel;
        }

        public void Update(int Id)
        {
            throw new NotImplementedException();
        }

        public void Delete(int Id)
        {
            throw new NotImplementedException();
        }

    }
}
