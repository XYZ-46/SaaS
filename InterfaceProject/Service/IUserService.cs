using DataEntity.User;

namespace InterfaceProject.Service
{
    public interface IUserService
    {
        public Task Register(UserRegisterRequest userRegisterParamReq);
    }
}
