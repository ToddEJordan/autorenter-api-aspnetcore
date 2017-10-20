using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoRenter.Api.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AutoRenter.Api.Authorization
{
    public class TokenManager : ITokenManager
    {
        private readonly AppSettings _appSettings;

        public DateTime UtcTime { get; set; }

        public TokenManager(IOptions<AppSettings> appSettings, DateTime utcTime)
        {
            _appSettings = appSettings.Value;
            UtcTime = utcTime;
        }
        public TokenManager(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            UtcTime = DateTime.Now.ToUniversalTime();
        }

        public virtual string CreateToken(UserModel userModel)
        {
            return new JwtSecurityTokenHandler().WriteToken(CreateJsonWebToken(userModel));
        }

        public virtual JwtSecurityToken CreateJsonWebToken(UserModel userModel)
        {
            return new JwtSecurityToken(
                new JwtHeader(GetSigningCredentials()),
                new JwtPayload(
                    _appSettings.TokenSettings.Issuer,
                    _appSettings.TokenSettings.Audience,
                    new Claim[]{
                        new Claim("username", userModel.Username),
                        new Claim(JwtRegisteredClaimNames.Sub, userModel.Id.ToString())
                    },
                    UtcTime,
                    UtcTime.AddMinutes(_appSettings.TokenSettings.ExpirationMinutes),
                    UtcTime));
        }

        public virtual SigningCredentials GetSigningCredentials()
        {
            return new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appSettings.TokenSettings.Secret)),
                SecurityAlgorithms.HmacSha256);
        }

        public virtual bool IsTokenValid(string token)
        {
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidAudiences = new[] { _appSettings.TokenSettings.Audience },
                    ValidIssuers = new[] { _appSettings.TokenSettings.Issuer },
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appSettings.TokenSettings.Secret))
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, tokenValidationParameters, out var _);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
