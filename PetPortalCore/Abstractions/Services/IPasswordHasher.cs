namespace PetPortalCore.Abstractions.Services;

/// <summary>
/// Интерфейс для хеширования и проверки паролей.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Хэшировать пароль.
    /// </summary>
    /// <param name="password">Пароль для хеширования.</param>
    /// <returns>Хэшированный пароль.</returns>
    string HashPassword(string password);

    /// <summary>
    /// Проверить соответствие пароля его хэшу.
    /// </summary>
    /// <param name="hashedPassword">Хэшированный пароль.</param>
    /// <param name="providedPassword">Пароль для проверки.</param>
    /// <returns>
    /// True - если пароль соответствует хэшу;
    /// False - если пароль не соответствует хэшу.
    /// </returns>
    bool VerifyHashedPassword(string hashedPassword, string providedPassword);
}