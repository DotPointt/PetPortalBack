using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.IdentityModel.Tokens;
using PetPortalApplication.Services;
using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.Abstractions.Services;
using PetPortalDAL.Repositories;
using System.Text;

namespace PetPortalAPI
{
    public abstract class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // ���������, ����� �� �������������� �������� ��� ��������� ������
                        ValidateIssuer = true,
                        // ������, �������������� ��������
                        ValidIssuer = AuthOptions.ISSUER,
                        // ����� �� �������������� ����������� ������
                        ValidateAudience = true,
                        // ��������� ����������� ������
                        ValidAudience = AuthOptions.AUDIENCE,
                        // ����� �� �������������� ����� �������������
                        ValidateLifetime = true,
                        // ��������� ����� ������������
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        // ��������� ����� ������������
                        ValidateIssuerSigningKey = true,
                    };
                });
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            builder.Services.AddScoped<IProjectsService, ProjectService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddSingleton<IProjectsRepository, ProjectRepository>();

            var app = builder.Build();

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


public class AuthOptions
{
    public const string ISSUER = "MyAuthServer"; // �������� ������
    public const string AUDIENCE = "MyAuthClient"; // ����������� ������
    const string KEY = "mysupersecret_secretsecretsecretkey!123";   // ���� ��� ��������
    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}