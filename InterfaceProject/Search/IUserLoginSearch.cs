using DataEntity.User;

namespace InterfaceProject.Search
{
    public interface IUserLoginSearch
    {
        public Task<UserLoginModel?> FindByIdAsync(int Id);
        public Task<UserLoginModel?> FindByUsernameAsync(string username);
    }
}
