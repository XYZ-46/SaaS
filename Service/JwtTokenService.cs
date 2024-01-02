using AppConfiguration;
using DataEntity.User;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InterfaceProject.Service;


namespace Service
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtSetting _jwtSetting;

        public JwtTokenService(IOptions<JwtSetting> jwtSetting)
        {
            _jwtSetting = jwtSetting.Value;
        }
        public string GenerateJwtTokenAsync(UserLoginParam user)
        {
            var SecretKey = Encoding.ASCII.GetBytes(_jwtSetting.Secret);
            var SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(SecretKey), SecurityAlgorithms.HmacSha256);

            var Claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "UserEmail"),
                new Claim(JwtRegisteredClaimNames.GivenName, "UserName"),
                new Claim(JwtRegisteredClaimNames.Jti, "UserID")
            };

            var SecurityToken = new JwtSecurityToken(
                    issuer: _jwtSetting.Issuer,
                    audience: _jwtSetting.Audience,
                    expires: DateTime.UtcNow.AddHours(_jwtSetting.ExpiryHours),
                    claims: Claims,
                    signingCredentials: SigningCredentials
                );

            return new JwtSecurityTokenHandler().WriteToken(SecurityToken);
        }

        public int? ValidateJwtToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}
