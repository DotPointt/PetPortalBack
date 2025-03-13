namespace PetPortalCore.Contracts;

/// <summary>
/// Контракт для создания пользователя.
/// </summary>
public class UserContract
{
    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Электронная почта пользователя.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Пароль пользователя.
    /// </summary>
    public string Password { get; set; }
}