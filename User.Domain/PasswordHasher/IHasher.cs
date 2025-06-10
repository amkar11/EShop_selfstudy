using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.PasswordHasher
{
    public interface IHasher
    {
        string Hash(string password);
        bool Verify(string HashPassword, string InputPassword);
    }
}
