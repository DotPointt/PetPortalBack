namespace PetPortalApplication.AuthConfiguration;

/// <summary>
/// Класс для хранения настроек JWT (JSON Web Token).
/// </summary>
public class JwtOptions
{
    /// <summary>
    /// Секретный ключ, используемый для подписи и проверки JWT.
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>
    /// Время жизни токена в часах.
    /// </summary>
    public int ExpiresHours { get; set; }
}