using AppConfiguration;
using DataEntity.User;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InterfaceProject.Service;
using System;
using System.Security.Cryptography;
using DataEntity;


namespace Service
{
    public class JwtTokenService(IOptions<JwtSetting> jwtSetting) : IJwtTokenService
    {
        private readonly JwtSetting _jwtSetting = jwtSetting.Value;

        public string GenerateJwtToken(UserLoginModel userLogin, UserProfileModel userProfile)
        {
            var SecretKey = Encoding.ASCII.GetBytes(_jwtSetting.Secret);
            var SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(SecretKey), SecurityAlgorithms.HmacSha256);

            var Claims = new List<Claim>
            {
                new("id", userLogin.Id.ToString()),
                new("email", userProfile.Email),
                new("FullName", userProfile.Fullname),
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

        public bool ValidateJwtToken(string token)
        {
            throw new NotImplementedException();
        }

        public static RefreshToken GenerateRefreshToken(string ipAddress)
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomBytes),
                Expires = DateTime.UtcNow.AddMinutes(2),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }
    }
}
