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
using PetPortalAPI.Options;
using PetPortalApplication.AuthConfiguration;
using PetPortalCore.Configs;
using PetPortalDAL;
using PetPortalDAL.Mappers;
using PetPortalDAL.Repositories;

namespace PetPortalAPI
{
    public abstract class Program
    {
        public static async Task Main(string[] args)
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
                    await context.Database.MigrateAsync(); // Применяет миграции
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }


            await ConfigureApp(app);
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
                            // SignalR может передавать токен в query; иначе — cookie
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chat"))
                            {
                                context.Token = accessToken;
                            }
                            else
                            {
                                context.Token = context.Request.Cookies["jwttoken"];
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
            
            #endregion 
            
            #region Конфигурации
            
            // Регистрация конфигураций из appsettings.json
            services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions))); // Конфигурация JWT
            services.Configure<MinIOConfig>(configuration.GetSection("MinioConfig")); // Конфигурация MinIO
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
                // Path.Combine вместо '\' в пути - иначе XML-доки не находятся в Linux-контейнере
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "PetPortalAPI.xml"));

                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "PetPortalCore.xml"));
            });

            // Настройка контекста базы данных
            services.AddDbContext<PetPortalDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString(nameof(PetPortalDbContext))); 
            });

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
            services.AddScoped<IEmailConfirmationService, EmailConfirmationService>();
            services.AddScoped<IUnreadChatEmailService, UnreadChatEmailService>();
            services.AddHostedService<UnreadChatEmailBackgroundService>();
            services.AddScoped<IRespondService, RespondService>();
            services.AddScoped<IRolesService, RoleService>();
            services.AddSingleton<IRabbitMqProducerService, RabbitMqProducerService>();

            // Регистрация репозиториев
            services.AddScoped<IProjectsRepository, ProjectsRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IUserProjectRepository, UserProjectRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
            services.AddScoped<IResetPasswordTokensRepository, ResetPasswordTokensRepository>();
            services.AddScoped<IEmailConfirmationTokensRepository, EmailConfirmationTokensRepository>();
            services.AddScoped<IChatNotificationRepository, ChatNotificationRepository>();
            services.AddScoped<IChatRoomRepository, ChatRoomRepository>();
            services.AddScoped<IStackRepository, StackRepository>();
            services.AddScoped<IExperienceRepository, ExperienceRepository>();
            services.AddScoped<IEducationRepository, EducationRepository>();
            services.AddScoped<IRespondRepository, RespondRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            // Регистрация вспомогательных сервисов
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddMapster();
            
            //Регистрация Настроек
            services.Configure<RabbitMqOptions>( configuration.GetSection("RabbitMq"));
            
            #endregion
            
            // Настройка CORS (Cross-Origin Resource Sharing)
            // Источники можно переопределить через конфигурацию: Cors:AllowedOrigins (адреса через ';')
            var allowedOrigins = configuration.GetValue<string>("Cors:AllowedOrigins")
                ?.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                ?? new[] { "http://localhost:5173", "http://localhost:5174" };

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", builder =>
                {
                    builder.WithOrigins(allowedOrigins) // Разрешенные источники
                        .AllowAnyHeader() // Разрешение любых заголовков
                        .AllowAnyMethod() // Разрешение любых методов
                        .AllowCredentials(); // Разрешение учетных данных
                });
            });
        }

        /// <summary>
        /// Настройка приложения.
        /// </summary>
        /// <param name="app">Экземпляр приложения.</param>
        private static async Task ConfigureApp(WebApplication app)
        {
            // Включение Swagger: в Development всегда, в остальных окружениях — при EnableSwagger=true
            if (app.Environment.IsDevelopment() || app.Configuration.GetValue<bool>("EnableSwagger"))
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();
                var minioService = services.GetRequiredService<IMinioService>();

                // Бакет MinIO нужен в любом окружении, операции идемпотентны
                try
                {
                    await minioService.EnsureBucketExistsAsync();
                    await minioService.MakeBucketPublicAsync();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while initializing the MinIO bucket.");
                }

                // Тестовые данные: в Development всегда, в остальных окружениях — при SeedDatabase=true
                if (app.Environment.IsDevelopment() || app.Configuration.GetValue<bool>("SeedDatabase"))
                {
                    try
                    {
                        var context = services.GetRequiredService<PetPortalDbContext>();
                        await DbInitializer.Seed(context, minioService);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred while seeding the database.");
                    }
                }
            }

            // Перенаправление на HTTPS
            app.UseHttpsRedirection();
            
            app.UseCors("AllowSpecificOrigin");
            
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


            // Запуск приложения
            await app.RunAsync();
        }
    }
}