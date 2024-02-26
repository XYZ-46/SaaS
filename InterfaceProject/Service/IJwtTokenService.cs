using DataEntity.Model;

namespace InterfaceProject.Service
{
    public interface IJwtTokenService
    {
        string GenerateJwtToken(UserProfileModel userProfile);
        (bool, int) ValidateJwtToken(string token);
    }
}
