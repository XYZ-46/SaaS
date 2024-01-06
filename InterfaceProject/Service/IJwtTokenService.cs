using DataEntity.User;

namespace InterfaceProject.Service
{
    public interface IJwtTokenService
    {
        public string GenerateJwtToken(UserLoginModel userLogin, UserProfileModel userProfile);
        public bool ValidateJwtToken(string token);
    }
}
