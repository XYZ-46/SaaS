using DataEntity.User;

namespace InterfaceProject.Service
{
    public interface IAuthService : IDisposable
    {
        public Task<string> Login(LoginRequest userloginParam);
        public Task ForgotPassword();

    }
}
