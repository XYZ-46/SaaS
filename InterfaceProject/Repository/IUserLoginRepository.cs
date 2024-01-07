using DataEntity.User;

namespace InterfaceProject.Repository
{
    public interface IUserLoginRepository : IBaseCrudRepository<UserLoginModel>
    {
        public Task<UserLoginModel?> FindByUsernameAsync(string username);
    }
}
