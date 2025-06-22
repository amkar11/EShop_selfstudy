using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace User.Application
{
    public interface IJwtTokenValidator
    {
        bool IsTokenValid(string token);
    }
}
