using DataEntity.User;

namespace InterfaceProject.Repository
{
    public interface IUserLoginRepo
    {
        public Task<UserLoginModel> Insert(UserLoginModel userLoginModel);
        public void Update(int Id);
        public void Delete(int Id);
    }
}
