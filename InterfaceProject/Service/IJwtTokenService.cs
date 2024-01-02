using DataEntity.User;

namespace InterfaceProject.Service
{
    public interface IJwtTokenService
    {
        public string GenerateJwtTokenAsync(UserLoginParam user);
        public int? ValidateJwtToken(string token);
    }
}
