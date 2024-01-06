using DataEntity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceProject.Repository
{
    public interface IUserProfileRepo : IDisposable
    {
        public Task<UserProfileModel> InsertAsync(UserProfileModel UserProfileModel);
        public void Update(int Id);
        public void Delete(int Id);
    }
}
