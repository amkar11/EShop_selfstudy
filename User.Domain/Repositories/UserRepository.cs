using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using User.Domain.Database;
using User.Domain.Models;
using User.Domain.PasswordHasher;

namespace User.Domain.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHasher _hasher;
        public UserRepository(ApplicationDbContext context, IHasher hasher)
        {
            _context = context;
            _hasher = hasher;
        }

        public async Task AddUserAsync(UserDb user)
        {
           await _context.Users.AddAsync(user);
           await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            UserDb? user = await _context.Users.FindAsync(id);
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            _context.Users.Remove(user);    
            await _context.SaveChangesAsync();
        }

        public async Task<UserDb> GetUserByIdAsync(int id)
        {
            UserDb? user = await _context.Users.FindAsync(id);
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            return user;
        }

        public async Task<UserDb> GetUserByNameAndPasswordAsync(string userName, string password)
        {
            UserDb? user = await _context.Users.FirstOrDefaultAsync(x => x.Name == userName);
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            return user;
        }
        public async Task<UserDb?> GetUserByNameOrEmailAsync(string userNameOrEmail)
        {
            UserDb? user = await _context.Users.FirstOrDefaultAsync(x => x.Name == userNameOrEmail || x.Email == userNameOrEmail);
            return user;
        }

        public async Task UpdateUserAsync(UserDb user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
