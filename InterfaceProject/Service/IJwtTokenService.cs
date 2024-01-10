using DataEntity.User;

namespace InterfaceProject.Service
{
    public interface IJwtTokenService
    {
        public string GenerateJwtToken(UserProfileModel userProfile);
        public bool ValidateJwtToken(string token);
    }
}
