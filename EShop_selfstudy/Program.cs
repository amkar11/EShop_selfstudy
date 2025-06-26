using AdditionalTools.InMemoryCache;
using EShop_selfstudy.Data;
using EShop_selfstudy.Data.Interfaces;
using EShop_selfstudy.Data.Models;
using EShop_selfstudy.Data.Repository;
using Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using User.Domain.Models;

namespace EShop_selfstudy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            //Repositories
            builder.Services.AddScoped<IRepository, Repository>();
            builder.Services.AddScoped<IAllCars,CarRepository>();
            builder.Services.AddScoped<ICarsCategory, CategoryRepository>();
            builder.Services.AddScoped<IAllOrders, OrdersRepository>();

            //Mapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            //Cache
            builder.Services.AddMemoryCache();
            builder.Services.AddSingleton<ICacheService, CacheService>();

            //SQL
            builder.Configuration.AddJsonFile("dbcontext.json", optional: false, reloadOnChange: true);
            builder.Services.AddDbContext<AppDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //Session
            builder.Services.AddSession();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            //Loggers
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();

            //HTTP client for ShoppingCart
            builder.Services.AddHttpClient("ShoppingCart", client =>
            {
                client.BaseAddress = new Uri("http://localhost:5176");
            });

            builder.Services.AddHttpClient("AuthClient", client =>
            {
                client.BaseAddress = new Uri("http://localhost:5090");
            });

            //JWT Config
            var path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../User/User_EShop"));
            var config = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var jwtSettings = config.GetSection("Jwt");
            builder.Services.Configure<JwtSettings>(jwtSettings);

            //Authentication and autharization
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var rsa = RSA.Create();
                rsa.ImportFromPem(File.ReadAllText("../data/public.key"));
                var public_key = new RsaSecurityKey(rsa);

                var jwtConfig = jwtSettings.Get<JwtSettings>();
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfig!.Issuer,
                    ValidAudience = jwtConfig.Audience,
                    IssuerSigningKey = public_key,
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var access_token = context.Request.Cookies["access_token"];
                        if (!string.IsNullOrEmpty(access_token))
                        {
                            context.Token = access_token;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Administrator"));
                options.AddPolicy("EmployeeOnly", policy => policy.RequireRole("Employee"));
                options.AddPolicy("ClientOnly", policy => policy.RequireRole("Client"));
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                AppDBContext context = scope.ServiceProvider.GetRequiredService<AppDBContext>();
                DBObjects.Initial(context);
            }


            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "categoryFilter",
                pattern: "{controller=Cars}/{action}/{category?}",
                defaults: new {controller = "Cars", action= "List"});
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.Run();
        }
    }
}
