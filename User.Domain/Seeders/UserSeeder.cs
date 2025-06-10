using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Database;
using User.Domain.Models;
using User.Domain.PasswordHasher;

namespace User.Domain.Seeders
{
    public class UserSeeder : IUserSeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly IHasher _hasher;
        public UserSeeder(ApplicationDbContext context, IHasher hasher)
        {
            _context = context;
            _hasher = hasher;
        }
        public async Task InitializeAsync()
        {
            if (_context.Users.Any()) return;
            var admin = new UserDb
            {
                Name = "aleksafleksa",
                Email = "EShop_Cars@outlook.com",
                PhoneNumber = "+48501567866",
                PasswordHash = _hasher.Hash("aleksafleksaMVP"),
                Country = "Poland",
                Region = "malopolskie",
                Settlement = "Zakopane",
                PostalCode = "38-340",
                IsEmailConfirmed = true

            };
            await _context.Users.AddAsync(admin);
            
            await _context.SaveChangesAsync();
        }
    }
}
