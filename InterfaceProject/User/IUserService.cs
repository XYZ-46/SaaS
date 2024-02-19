using DataEntity.Request;

namespace InterfaceProject.User
{
    public interface IUserService
    {
        public Task Register(UserRegisterRequest userRegisterParam);
    }
}
