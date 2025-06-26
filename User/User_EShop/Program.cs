using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using User.Application;
using User.Domain.Database;
using User.Domain.Models;
using User.Domain.PasswordHasher;
using User.Domain.Repositories;
using AdditionalTools.Extensions;
using User.Domain.Seeders;
using Extensions;
using AdditionalTools.EmailSender;
using AdditionalTools.InMemoryCache;

namespace User_EShop
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //dbcontext.json
            builder.Configuration.AddJsonFile("dbcontext.json", optional: false, reloadOnChange: true);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            //Hasher
            builder.Services.AddTransient<IHasher, Hasher>();

            //ShoppingCart Client
            builder.Services.AddHttpClient("ShoppingCart", client =>
            {
                client.BaseAddress = new Uri("http://localhost:5176");
            });

            //EmailSender
            builder.Services.AddTransient<IEmailSender, EmailSender>();

            //In-memory cache
            builder.Services.AddSingleton<ICacheService, CacheService>();

            //Session
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            //SQLServer
            var cs = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
            if (string.IsNullOrEmpty(cs)) throw new NullReferenceException("Connection string not found!");
                options.UseSqlServer(cs);
        });
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            //Login
            builder.Services.AddScoped<ILoginService, LoginService>();

            //Registartion
            builder.Services.AddScoped<IRegistrationService, RegistrationService>();

            //Distributed Cache
            builder.Services.AddDistributedMemoryCache();
            
            //Loggers
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();

            //JWT token validation
            builder.Services.AddScoped<IJwtTokenValidator, JwtTokenValidator>();


            // JWT Configuration
            var jwtSettings = builder.Configuration.GetSection("Jwt");
            builder.Services.Configure<JwtSettings>(jwtSettings);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var config = jwtSettings.Get<JwtSettings>();

                var rsa = RSA.Create();
                rsa.ImportFromPem(File.ReadAllText("../../data/public.key"));
                var public_key = new RsaSecurityKey(rsa);
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config!.Issuer,
                    ValidAudience = config.Audience,
                    IssuerSigningKey = public_key
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Cookies["access_token"];
                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly",
                    policy => policy.RequireRole("Administrator"));
            });

            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
            

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var hasher = scope.ServiceProvider.GetRequiredService<IHasher>();
                var seeder = new UserSeeder(context, hasher);
                await seeder.InitializeAsync();
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                await app.ApplyMigrationsAsync();
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Login}/{action=LoginView}/{id?}");

            app.MapControllers();

            app.Run();
        }
    }
}
