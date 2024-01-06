using DataEntity.User;
using InterfaceProject.Repository;
using Repository.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UserProfileRepo(AzureDB azureDB) : IUserProfileRepo
    {
        private readonly AzureDB _azureDB = azureDB;

        public void Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<UserProfileModel> InsertAsync(UserProfileModel UserProfileModel)
        {
            _azureDB.UserProfileModel.Add(UserProfileModel);
            await _azureDB.SaveChangesAsync();
            return UserProfileModel;
        }

        public void Update(int Id)
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
