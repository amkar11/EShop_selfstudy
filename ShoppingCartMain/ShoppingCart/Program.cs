using Extensions;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Application.Services;
using ShoppingCart.Domain.Database;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Infrastructure.Repositories;

namespace ShoppingCart
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //MediatR
            builder.Services.AddMediatR(cfg => 
                cfg.RegisterServicesFromAssembly(typeof(ShoppingCart.Application.Services.CartService).Assembly));

            //Repository and ICart interfaces
            builder.Services.AddScoped<ICartRepository, CartRepository>();
            builder.Services.AddScoped<IShopCartItemRepository, ShopCartItemRepository>();
            builder.Services.AddScoped<ICartProductAdder, CartService>();
            builder.Services.AddScoped<ICartProductRemover, CartService>();
            builder.Services.AddScoped<ICartReader, CartService>();
            builder.Services.AddScoped<IProductQuantityChanger, CartService>();
            builder.Services.AddScoped<ICartCreater, CartService>();
            builder.Services.AddScoped<ICartRemover, CartService>();
            builder.Services.AddScoped<ICartProductReader, CartService>();
            builder.Services.AddScoped<ICartUpdater, CartService>();

            //AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            //SQL Server
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
