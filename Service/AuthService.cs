using DataEntity.User;
using InterfaceProject.Service;
using BCryptNet = BCrypt.Net.BCrypt;
using System.Security.Authentication;
using InterfaceProject.Repository;

namespace Service
{
    public class AuthService(IJwtTokenService jwtTokenService, IUserProfileRepository userProfileRepo)
        : IAuthService
    {
        private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
        public readonly IUserProfileRepository _userProfileRepo = userProfileRepo;

        public async Task ForgotPassword()
        {
            throw new NotImplementedException();
        }

        public async Task<string> Login(LoginRequest userloginParam)
        {
            UserProfileModel userprofile = await _userProfileRepo.FindByUserLoginUsernameAsync(userloginParam.Username) ?? throw new MissingMemberException("User do not setup yet");

            if (!BCryptNet.Verify(userloginParam.Password, userprofile.UserLogin.PasswordHash))
                throw new AuthenticationException("Incorrect Username or password");

            var token = _jwtTokenService.GenerateJwtToken(userprofile);
            return token;
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                _jwtTokenService.Dispose();
                _userProfileRepo.Dispose();
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
