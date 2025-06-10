using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using User.Domain.Models;

namespace User.Application
{
    public class JwtTokenValidator : IJwtTokenValidator
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<JwtTokenValidator> _logger;
        public JwtTokenValidator(IConfiguration configuration, ILogger<JwtTokenValidator> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public bool IsTokenValid(string token)
        {
            if (string.IsNullOrEmpty(token)) return false;

            var tokenHandler = new JwtSecurityTokenHandler();
            RsaSecurityKey key = GetPublicSecurityKey();

            var jwtSettings = _configuration.GetSection("Jwt");
            var config = jwtSettings.Get<JwtSettings>();

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config!.Issuer,
                    ValidAudience = config!.Audience,
                    IssuerSigningKey = key,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                _logger.LogInformation("Token validation completed succefully");
                return true;
            }
            catch (Exception) {
                _logger.LogWarning("Token validation failed, \"Remember me\" token doesn`t exist, otherwise something wrong with method");
                return false;
            }
        }

        public static RsaSecurityKey GetPublicSecurityKey()
        {
            var rsa = RSA.Create();
            rsa.ImportFromPem(File.ReadAllText("../data/public.key"));
            return new RsaSecurityKey(rsa);
        }
    }
}
