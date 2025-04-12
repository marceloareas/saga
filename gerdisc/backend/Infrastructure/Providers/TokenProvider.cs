using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using saga.Models.Entities;
using saga.Models.Enums;
using saga.Properties;
using Microsoft.IdentityModel.Tokens;

namespace saga.Infrastructure.Providers
{
    /// <summary>
    /// Generates a token.
    /// </summary>
    public class TokenProvider : ITokenProvider
    {
        private readonly ISigningConfiguration _singingConfig;
        public TokenProvider(ISigningConfiguration singingConfig)
        {
            _singingConfig = singingConfig ?? throw new ArgumentNullException(nameof(singingConfig));
        }

        /// <inheritdoc />
        public string GenerateJwtToken(UserEntity user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()??""),
                    new Claim(ClaimTypes.Name, user.FirstName??""),
                    new Claim(ClaimTypes.Surname, user.LastName??""),
                    new Claim(ClaimTypes.Email, user.Email??""),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(50),
                SigningCredentials = new SigningCredentials(_singingConfig.Key, SecurityAlgorithms.RsaSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <inheritdoc />
        public string GenerateResetPasswordJwt(UserEntity user, TimeSpan durationTime)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()??""),
                    new Claim(ClaimTypes.Name, user.FirstName??""),
                    new Claim(ClaimTypes.Surname, user.LastName??""),
                    new Claim(ClaimTypes.Email, user.Email??""),
                    new Claim(ClaimTypes.Role, RolesEnum.ResetPassword.ToString())
                }),
                Expires = DateTime.UtcNow + durationTime,
                SigningCredentials = new SigningCredentials(_singingConfig.Key, SecurityAlgorithms.RsaSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
