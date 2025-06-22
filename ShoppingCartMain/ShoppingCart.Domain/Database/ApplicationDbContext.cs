using Microsoft.EntityFrameworkCore;
using ShoppingCart.Domain.Models;

namespace ShoppingCart.Domain.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<ShopCartItem> Products { get; set; }
    }
}
