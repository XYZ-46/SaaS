using DataEntity.User;

namespace InterfaceProject.Service
{
    public interface IJwtTokenService
    {
        public string GenerateJwtTokenAsync(LoginRequest user);
        public int? ValidateJwtToken(string token);
    }
}
