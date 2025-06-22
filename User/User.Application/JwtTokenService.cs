using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using User.Domain.Models;

namespace User.Application
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtSettings _settings;
        private readonly IConfiguration _configuration;
        public JwtTokenService(IOptions<JwtSettings> settings, IConfiguration configuration)
        {
            _settings = settings.Value;
            _configuration = configuration;
        }

        

        public string GenerateToken(int userId, List<string> roles, bool rememberMe = false)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            claims.Add(new Claim (ClaimTypes.UserData, rememberMe.ToString()));

            var rsa = RSA.Create();
            rsa.ImportFromPem(File.ReadAllText("../data/private.key"));
            var creds = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: rememberMe ? DateTime.UtcNow.AddDays(14) : DateTime.UtcNow.AddMinutes(_settings.ExpiresInMinutes),
                signingCredentials: creds);
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static string GenerateRefreshToken()
        {
            var rng = RandomNumberGenerator.Create();
            byte[] bytes = new byte[64];
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var public_key = JwtTokenValidator.GetPublicSecurityKey();

            var jwtConfig = _configuration.GetSection("Jwt");
            var config = jwtConfig.Get<JwtSettings>();

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false,
                IssuerSigningKey = public_key,
                ValidIssuer = config!.Issuer,
                ValidAudience = config.Audience,
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                if (validatedToken is not JwtSecurityToken jwt ||
                    !jwt.Header.Alg.Equals(SecurityAlgorithms.RsaSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
            
        }
    }
}
