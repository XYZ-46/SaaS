using DataEntity.User;

namespace InterfaceProject.Repository
{
    public interface IUserLoginRepo : IDisposable
    {
        public Task<UserLoginModel> InsertAsync(UserLoginModel userLoginModel);
        public void Update(int Id);
        public void Delete(int Id);
    }
}
