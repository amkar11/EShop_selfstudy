using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Models;

namespace User.Domain.Repositories
{
    public interface IUserRepository
    {
        Task AddUserAsync(UserDb user);
        Task<UserDb> GetUserByIdAsync(int id);
        Task<UserDb> GetUserByNameAndPasswordAsync(string userName, string password);
        Task<UserDb?> GetUserByNameOrEmailAsync(string usernameOrEmail);
        Task UpdateUserAsync(UserDb user);
        Task DeleteUserAsync(int id);

    }
}
