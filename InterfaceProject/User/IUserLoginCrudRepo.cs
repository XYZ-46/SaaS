using DataEntity.Model;

namespace InterfaceProject.User
{
    public interface IUserLoginCrudRepo : IBaseCrudRepository<UserLoginModel>
    {
        public Task<UserLoginModel?> FindByUsernameAsync(string username);
    }
}
