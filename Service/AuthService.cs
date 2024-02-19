using InterfaceProject.Service;
using BCryptNet = BCrypt.Net.BCrypt;
using System.Security.Authentication;
using DataEntity.Request;
using DataEntity.Model;
using InterfaceProject.User;

namespace Service
{
    public class AuthService(IJwtTokenService jwtTokenService, IUserProfileCrudRepo userProfileRepo)
        : IAuthService
    {
        private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
        public readonly IUserProfileCrudRepo _userProfileRepo = userProfileRepo;

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
    }
}
