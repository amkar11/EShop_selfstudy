using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace User.Application
{
    public interface IJwtTokenService
    {
        string GenerateToken(int userId, List<string> roles, bool rememberMe = false);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
