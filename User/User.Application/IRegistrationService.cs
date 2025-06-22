using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Models;

namespace User.Application
{
    public interface IRegistrationService
    {
        Task<bool> CheckIfUserExistsByUsernameAsync(string username);
        Task AddNewUserToDatabaseAsync(UserDb user);
    }
}
