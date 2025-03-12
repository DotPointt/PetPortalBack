using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.IdentityModel.Tokens;
using PetPortalApplication.Services;
using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.Abstractions.Services;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PetPortalApplication.AuthConfiguration;
using PetPortalCore.Configs;
using PetPortalDAL;
using PetPortalDAL.Repositories;

namespace PetPortalAPI
{
    public abstract class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();
            ConfigureApp(app);
        }

        /// <summary>
        /// Services registration.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <param name="configuration">Configuration.</param>
        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            #region Authorization
            
            services.AddAuthorizationBuilder()
                .AddPolicy("AdminOnly", policy => 
                    policy.RequireClaim(ClaimTypes.Role, "Admin"))
                .AddPolicy("UserOnly", policy => 
                    policy.RequireClaim(ClaimTypes.Role, "User"));
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.ISSUER,
                        ValidateAudience = true,
                        ValidAudience = AuthOptions.AUDIENCE,
                        ValidateLifetime = true,
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,
                    };
                    options.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["jwttoken"];
                            return Task.CompletedTask;
                        }
                    };
                });
            
            #endregion 
            
            #region Configs
            
            services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
            services.Configure<MinIOConfig>(configuration.GetSection("MinioConfig"));
            
            #endregion
            
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                c.IncludeXmlComments($@"{System.AppDomain.CurrentDomain.BaseDirectory}\PetPortalAPI.xml");
                
                c.IncludeXmlComments($@"{System.AppDomain.CurrentDomain.BaseDirectory}\PetPortalCore.xml");
            });

            services.AddDbContext<PetPortalDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString(nameof(PetPortalDbContext)));
            });

            #region DI
            services.AddScoped<IProjectsService, ProjectService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserProjectService, UserProjectService>();
            services.AddScoped<IMinioService, MinioService>();

            services.AddScoped<IProjectsRepository, ProjectsRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IUserProjectRepository, UserProjectRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            #endregion
            
            services.AddCors();
            
        }

        private static void ConfigureApp(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                using (var scope = app.Services.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<PetPortalDbContext>();
                    DbInitializer.Seed(context);  
                }
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.UseCors(corsbuilder => corsbuilder.AllowAnyOrigin()); //TODO: Поменять на более безопасное

            app.Run();
        }
    }
}


