using DataEntity.User;
using InterfaceProject.Repository;
using Repository.Database;

namespace Repository
{
    public class UserLoginRepo(AzureDB azureDB) : IUserLoginRepo
    {
        private readonly AzureDB _azureDB = azureDB;

        public async Task<UserLoginModel> InsertAsync(UserLoginModel userLoginModel)
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

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed && disposing) _azureDB.Dispose();
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
