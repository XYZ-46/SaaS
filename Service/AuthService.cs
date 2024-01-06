using DataEntity.User;
using InterfaceProject.Search;
using InterfaceProject.Service;
using BCryptNet = BCrypt.Net.BCrypt;
using System.Security.Authentication;

namespace Service
{
    public class AuthService(IUserLoginSearch userLoginSearch, IJwtTokenService jwtTokenService, IUserProfileSearch userProfileSearch)
        : IAuthService
    {
        private readonly IUserLoginSearch _userLoginSearch = userLoginSearch;
        private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
        public readonly IUserProfileSearch _userProfileSearch = userProfileSearch;

        public async Task ForgotPassword()
        {
            throw new NotImplementedException();
        }

        public async Task<string> Login(LoginRequest userloginParam)
        {
            var userlogin = await _userLoginSearch.FindByUsernameAsync(userloginParam.Username) ?? throw new AuthenticationException("Incorrect Username or password");
            var userprofile = await _userProfileSearch.FindByUserLoginIdAsync(userlogin.Id) ?? throw new MissingMemberException("User do not setup yet");

            if (!BCryptNet.Verify(userloginParam.Password, userlogin.PasswordHash))
                throw new AuthenticationException("Incorrect Username or password");

            var token = _jwtTokenService.GenerateJwtToken(userlogin, userprofile);
            return token;
        }
    }
}
