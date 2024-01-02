﻿using AppConfiguration;
using DataEntity.User;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InterfaceProject.Service;
using System;
using System.Security.Cryptography;


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

            var Claims = new List<Claim>
            {
                new("id", "UserID"),
                new("email", "UserEmail"),
                new("FullName", "UserFullName"),
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

        public RefreshToken generateRefreshToken(string ipAddress)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
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
}