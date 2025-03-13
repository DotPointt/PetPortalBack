using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace PetPortalApplication.AuthConfiguration;

/// <summary>
/// Класс для хранения настроек аутентификации и генерации ключа безопасности.
/// </summary>
public static class AuthOptions
{
    /// <summary>
    /// Издатель токена (Issuer).
    /// </summary>
    public const string ISSUER = "MyAuthServer"; 

    /// <summary>
    /// Потребитель токена (Audience).
    /// </summary>
    public const string AUDIENCE = "MyAuthClient"; 

    /// <summary>
    /// Секретный ключ для шифрования и подписи токена.
    /// </summary>
    const string KEY = "mysupersecret_secretsecretsecretkey!123";   

    /// <summary>
    /// Генерация симметричного ключа безопасности на основе секретного ключа.
    /// </summary>
    /// <returns>Симметричный ключ безопасности.</returns>
    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}