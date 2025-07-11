using System.Security.Claims;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PetPortalApplication.Services;
using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.Abstractions.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PetPortalAPI.Controllers;
using PetPortalAPI.Hubs;
using PetPortalApplication.AuthConfiguration;
using PetPortalCore.Configs;
using PetPortalDAL;
using PetPortalDAL.Mappers;
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

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<PetPortalDbContext>();
                    context.Database.Migrate(); // Применяет миграции
                    Console.WriteLine("DB MIGRATED =========================\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while migrating the database.");
                }
            }

            ConfigureApp(app);


            var minioConfig = builder.Configuration.GetSection("MinioConfig").Get<MinIOConfig>();
            Console.WriteLine($"MinioConfig loaded: {minioConfig != null}");
            if (minioConfig == null)
            {
                Console.WriteLine("Ошибка: MinioConfig не найден в appsettings.json");
                throw new InvalidOperationException("MinioConfig не загружен");
            }

        }

        /// <summary>
        /// Регистрация сервисов и конфигураций.
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        /// <param name="configuration">Конфигурация приложения.</param>
        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            #region Авторизация и аутентификация
            
            // Настройка политик авторизации
            services.AddAuthorizationBuilder()
                .AddPolicy("AdminOnly", policy => 
                    policy.RequireClaim(ClaimTypes.Role, "Admin")) // Только для администраторов
                .AddPolicy("UserOnly", policy => 
                    policy.RequireClaim(ClaimTypes.Role, "User")); // Только для пользователей
            
            // Настройка аутентификации через JWT
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true, // Проверка издателя токена
                        ValidIssuer = AuthOptions.ISSUER, // Указание допустимого издателя
                        ValidateAudience = true, // Проверка аудитории токена
                        ValidAudience = AuthOptions.AUDIENCE, // Указание допустимой аудитории
                        ValidateLifetime = true, // Проверка срока действия токена
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(), // Ключ для проверки подписи
                        ValidateIssuerSigningKey = true, // Проверка подписи токена
                    };
                    options.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            // Получение токена из cookies
                            context.Token = context.Request.Cookies["jwttoken"];
                            return Task.CompletedTask;
                        }
                    };
                });
            
            #endregion 
            
            #region Конфигурации
            
            // Регистрация конфигураций из appsettings.json
            services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions))); // Конфигурация JWT

            services.Configure<EmailConfig>(configuration.GetSection("SmtpSettings")); // Конфигурация SMTPMailSender
            
            services.AddStackExchangeRedisCache(options =>
            {
                var connection = configuration.GetConnectionString("Redis");
                options.Configuration = connection;
            });
            services.Configure<YooKassaConfig>(configuration.GetSection("YooKassaOptions"));

            #endregion

            // Регистрация контроллеров и Swagger
            services.AddControllers();
            services.AddSignalR();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "PetPortalAPI.xml"));
                
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "PetPortalCore.xml"));
            });

            // Настройка контекста базы данных
            services.AddDbContext<PetPortalDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString(nameof(PetPortalDbContext))); 
            });

            services.Configure<MinIOConfig>(configuration.GetSection("MinioConfig")); // Конфигурация MinIO

            #region Внедрение зависимостей (DI)

            // Регистрация сервисов
            services.AddScoped<IProjectsService, ProjectService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserProjectService, UserProjectService>();
            services.AddScoped<IMinioService, MinioService>(); 
            services.AddScoped<IMailSenderService, MailSenderService>();
            services.AddScoped<IChatMessageService, ChatMessageService>();
            services.AddScoped<IChatRoomService, ChatRoomService>();
            services.AddScoped<IPaymentService, YooKassaService>();
            services.AddScoped<IResetPasswordService, ResetPasswordService>();

            // Регистрация репозиториев
            services.AddScoped<IProjectsRepository, ProjectsRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IUserProjectRepository, UserProjectRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
            services.AddScoped<IResetPasswordTokensRepository, ResetPasswordTokensRepository>();
            services.AddScoped<IChatRoomRepository, ChatRoomRepository>();
            services.AddScoped<IStackRepository, StackRepository>();
            services.AddScoped<IExperienceRepository, ExperienceRepository>();
            services.AddScoped<IEducationRepository, EducationRepository>();

            // Регистрация вспомогательных сервисов
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddMapster();
            
            #endregion
            
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", builder =>
                {
                    builder.WithOrigins("http://localhost:5173", "http://localhost:4173") // Разрешенный источник
                        .AllowAnyHeader() // Разрешение любых заголовков
                        .AllowAnyMethod() // Разрешение любых методов
                        .AllowCredentials(); // Разрешение учетных данных
                });
            });
            
            // services.AddCors(options =>
            // {
            //     options.AddPolicy("AllowAnyOrigin", builder =>
            //     {
            //         builder.AllowAnyOrigin()  // Разрешает запросы с любого источника
            //             .AllowAnyHeader()  // Разрешает любые заголовки
            //             .AllowAnyMethod()  // Разрешает любые HTTP-методы
            //             .AllowCredentials();  // Разрешает передачу учетных данных
            //     });
            // });
            
        }

        /// <summary>
        /// Настройка приложения.
        /// </summary>
        /// <param name="app">Экземпляр приложения.</param>
        private static void ConfigureApp(WebApplication app)
        {
            // Включение Swagger в режиме разработки
            //if (app.Environment.IsDevelopment())
            //{
                app.UsePathBase("/api");
                app.UseSwagger();
                app.UseSwaggerUI();
                
                // Инициализация базы данных (если требуется)
                using (var scope = app.Services.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<PetPortalDbContext>();
                    DbInitializer.Seed(context);  
                }
            //}

            // Перенаправление на HTTPS
            app.UseHttpsRedirection();
            
            // Включение аутентификации и авторизации
            app.UseAuthentication();
            app.UseAuthorization();
            
            // Маппинг контроллеров
            app.MapControllers();

            // Устанавливаем путь для чатов.
            // TODO
            // разобраться с путями.
            app.MapHub<ChatHub>("/chat");
            
            // Включение CORS
            app.UseCors("AllowAnyOrigin");

            // Запуск приложения
            app.Run();
        }
    }
}