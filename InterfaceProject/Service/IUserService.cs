using DataEntity.Request;

namespace InterfaceProject.Service
{
    public interface IUserService
    {
        public Task Register(UserRegisterRequest userRegisterParam);
    }
}
