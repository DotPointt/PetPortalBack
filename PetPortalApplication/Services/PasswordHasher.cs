using Microsoft.Win32;
using PetPortalCore.Abstractions.Services;

namespace PetPortalApplication.Services;

/// <summary>
/// Сервис для хеширования и проверки паролей.
/// </summary>
public class PasswordHasher : IPasswordHasher
{
    /// <summary>
    /// Хеширование пароля.
    /// </summary>
    /// <param name="password">Пароль для хеширования.</param>
    /// <returns>Хэшированный пароль.</returns>
    public string HashPassword(string password) 
        => BCrypt.Net.BCrypt.EnhancedHashPassword(password); 
    
    /// <summary>
    /// Проверка соответствия пароля его хэшу.
    /// </summary>
    /// <param name="hashedPassword">Хэшированный пароль.</param>
    /// <param name="providedPassword">Пароль для проверки.</param>
    /// <returns>
    /// True - если пароль соответствует хэшу;
    /// False - если пароль не соответствует хэшу.
    /// </returns>
    public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
        => BCrypt.Net.BCrypt.EnhancedVerify(providedPassword, hashedPassword); 
}