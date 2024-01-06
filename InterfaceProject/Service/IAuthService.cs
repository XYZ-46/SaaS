using DataEntity.User;

namespace InterfaceProject.Service
{
    public interface IAuthService
    {
        public Task<string> Login(LoginRequest userloginParam);
        public Task ForgotPassword();

    }
}
