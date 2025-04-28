using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetPortalApplication.AuthConfiguration;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.Models;

namespace PetPortalApplication.Services;

/// <summary>
/// Сервис для генерации JWT (JSON Web Token).
/// </summary>
public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
    /// <summary>
    /// Настройки JWT, загруженные из конфигурации.
    /// </summary>
    private readonly JwtOptions _options = options.Value;

    /// <summary>
    /// Генерация JWT токена на основе данных пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="email">Электронная почта пользователя.</param>
    /// <param name="roleName">Роль пользователя.</param>
    /// <returns>Сгенерированный JWT токен.</returns>
    /// <exception cref="Exception">Исключение, если произошла ошибка при генерации токена.</exception>
    public string GenerateToken(Guid userId, string email, string roleName)
    {
        try
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),          // Стандартный claim для subject (user ID)
                new Claim(JwtRegisteredClaimNames.Email, email),                    // Стандартный claim для email
                new Claim(ClaimTypes.Role, roleName),                               // Роль (можно использовать JwtRegisteredClaimNames для кастомных)
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())   // Уникальный идентификатор токена
            };

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(_options.ExpiresHours),
                signingCredentials: new SigningCredentials(
                    AuthOptions.GetSymmetricSecurityKey(), 
                    SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
    }
}