using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using TaskManager_api.Data;
using TaskManager_api.Repositories;
using TaskManager_api.Services;
using TaskManager_api.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using DotNetEnv;

namespace TaskManager_api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Env.Load(); // Load .env fil
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Database context
            //builder.Services.AddDbContext<AppDbContext>(options =>
             //   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            // Đăng ký DbContext với connection từ .env
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Environment.GetEnvironmentVariable("DB_CONNECTION")));

            // Repository & Service DI
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();

            // Jwt Helper DI (Singleton)
            builder.Services.AddSingleton<JwtHelper>(new JwtHelper(
                builder.Configuration["Jwt:Key"] ?? "super-secret-key"
            ));

            // ===== Authentication (JWT) =====
            // Cần using Microsoft.AspNetCore.Authentication.JwtBearer;
            var jwtKey = builder.Configuration["Jwt:Key"] ?? "super-secret-key";
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; // dev: true in production
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });

            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                if (db.Database.CanConnect())
                {
                    Console.WriteLine("✅ Database connected successfully!");
                }
                else
                {
                    Console.WriteLine("❌ Failed to connect to database.");
                }
            }
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Authentication trước Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
