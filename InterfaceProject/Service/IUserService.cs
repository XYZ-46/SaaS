using DataEntity.User;

namespace InterfaceProject.Service
{
    public interface IUserService
    {
        public void Register(UserRegisterRequest userRegisterParamReq);
    }
}
