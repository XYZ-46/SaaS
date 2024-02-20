using DataEntity.Model;

namespace InterfaceProject.User
{
    public interface IUserLoginRepository : IBaseRepository<UserLoginModel>
    {
        public Task<UserLoginModel?> FindByUsernameAsync(string username);
    }
}
