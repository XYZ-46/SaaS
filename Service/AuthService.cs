using DataEntity.User;
using InterfaceProject.Service;
using BCryptNet = BCrypt.Net.BCrypt;
using System.Security.Authentication;
using InterfaceProject.Repository;

namespace Service
{
    public class AuthService(IUserLoginRepository userLoginRepo, IJwtTokenService jwtTokenService, IUserProfileRepository userProfileRepo)
        : IAuthService
    {
        private readonly IUserLoginRepository _userLoginRepo = userLoginRepo;
        private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
        public readonly IUserProfileRepository _userProfileRepo = userProfileRepo;

        public async Task ForgotPassword()
        {
            throw new NotImplementedException();
        }

        public async Task<string> Login(LoginRequest userloginParam)
        {
            var userlogin = await _userLoginRepo.FindByUsernameAsync(userloginParam.Username) ?? throw new AuthenticationException("Incorrect Username or password");
            var userprofile = await _userProfileRepo.FindByUserLoginIdAsync(userlogin.Id) ?? throw new MissingMemberException("User do not setup yet");

            if (!BCryptNet.Verify(userloginParam.Password, userlogin.PasswordHash))
                throw new AuthenticationException("Incorrect Username or password");

            var token = _jwtTokenService.GenerateJwtToken(userlogin, userprofile);
            return token;
        }
    }
}
