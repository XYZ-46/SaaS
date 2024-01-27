using DataEntity.Model;

namespace InterfaceProject.Repository
{
    public interface IUserLoginRepository : IBaseCrudRepository<UserLoginModel>
    {
        public Task<UserLoginModel?> FindByUsernameAsync(string username);
    }
}
