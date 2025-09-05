using DotNetEnv;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using TaskManager_api.Data;
using TaskManager_api.Helpers;
using TaskManager_api.Repositories.Auth;
using TaskManager_api.Repositories.BoardColumns;
using TaskManager_api.Repositories.Boards;
using TaskManager_api.Repositories.Projects;
using TaskManager_api.Repositories.ProjectUsers;
using TaskManager_api.Repositories.Users;
using TaskManager_api.Services.Auth;
using TaskManager_api.Services.BoardColumns;
using TaskManager_api.Services.Boards;
using TaskManager_api.Services.Projects;
using TaskManager_api.Services.ProjectUsers;
using TaskManager_api.Services.Users;

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
            builder.Services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            });
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            // Database context
            builder.Services.AddDbContext<AppDbContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("DBCONNECTION")));
            // Đăng ký DbContext với connection từ .env
           // builder.Services.AddDbContext<AppDbContext>(options =>
            //    options.UseSqlServer(Environment.GetEnvironmentVariable("DB_CONNECTION")));

            // Repository & Service DI
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IProjectService, ProjectService>();
            builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
            builder.Services.AddScoped<IProjectUserRepository, ProjectUserRepository>();
            builder.Services.AddScoped<IProjectUserService, ProjectUserService>();
            builder.Services.AddScoped<IBoardRepository, BoardRepository>();
            builder.Services.AddScoped<IBoardService, BoardService>();
            builder.Services.AddScoped<IBoardColumnRepository, BoardColumnRepository>();
            builder.Services.AddScoped<IBoardColumnService, BoardColumnService>();

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
