using Microsoft.EntityFrameworkCore;
using User.Domain.Models;

namespace User.Domain.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<UserDb> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<RefreshTokenDb> RefreshTokens { get; set; }
    }
}
