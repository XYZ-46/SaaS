using DataEntity.Request;

namespace InterfaceProject.Service
{
    public interface IAuthService
    {
        public Task<string> Login(LoginRequest userloginParam);
        public Task ForgotPassword();

    }
}
