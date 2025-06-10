using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.Models
{
    public record TokenResponse(string accessToken = default!, string refreshToken = default!);
    
}
