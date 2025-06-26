using EShop_selfstudy.Data.Models;
using Microsoft.EntityFrameworkCore;
namespace EShop_selfstudy.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) :base(options) { }

        public DbSet<Car> Car {  get; set; }
        public DbSet<Category> Categories { get; set; } 
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }

    }
}
