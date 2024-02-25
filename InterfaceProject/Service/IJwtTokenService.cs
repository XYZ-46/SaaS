using DataEntity.Model;

namespace InterfaceProject.Service
{
    public interface IJwtTokenService
    {
        string GenerateJwtToken(UserProfileModel userProfile);
        Task<bool> ValidateJwtToken(string token);
    }
}
