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
    
    /// <summary>
    /// Страна проживания.
    /// </summary>
    public string Country { get; set; }
    
    /// <summary>
    /// Город проживания.
    /// </summary>
    public string City { get; set; }
    
    /// <summary>
    /// Номер телефона.
    /// </summary>
    public string Phone { get; set; }
    
    /// <summary>
    /// Телеграм аккаунт.
    /// </summary>
    public string Telegram { get; set; }
}