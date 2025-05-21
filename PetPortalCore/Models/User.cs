namespace PetPortalCore.Models;

/// <summary>
/// Класс, представляющий пользователя.
/// </summary>
public class User
{
    /// <summary>
    /// Конструктор для создания пользователя.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="name">Имя пользователя.</param>
    /// <param name="email">Электронная почта пользователя.</param>
    /// <param name="passwordHash">Хэш пароля пользователя.</param>
    /// <param name="roleId">Идентификатор роли пользователя.</param>
    /// <param name="avatarUrl">Путь к аватару пользователя.</param>
    public User(Guid id, string name, string email, string passwordHash, Guid roleId, string avatarUrl, 
        string country = "", string city = "", string phone = "", string? telegram = "")
    {
        Id = id;
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        RoleId = roleId;
        Country = country ?? string.Empty; 
        City = city ?? string.Empty; 
        Phone = phone ?? string.Empty; 
        Telegram = telegram ?? string.Empty; 
        AvatarUrl = avatarUrl ?? string.Empty; 
    }
        
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid Id;

    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public string Name = string.Empty;

    /// <summary>
    /// Электронная почта пользователя.
    /// </summary>
    public string Email = string.Empty;

    /// <summary>
    /// Хэш пароля пользователя.
    /// </summary>
    public string PasswordHash = string.Empty;
    
    /// <summary>
    /// Страна проживания.
    /// </summary>
    public string Country = string.Empty;
    
    /// <summary>
    /// Город проживания.
    /// </summary>
    public string City = string.Empty;
    
    /// <summary>
    /// Номер телефона.
    /// </summary>
    public string Phone = string.Empty;
    
    /// <summary>
    /// Телеграм аккаунт.
    /// </summary>
    public string Telegram = string.Empty;
    
    /// <summary>
    /// Идентификатор роли пользователя.
    /// </summary>
    public Guid RoleId = Guid.Empty;
    
    /// <summary>
    /// Путь к аватарке пользователя.
    /// </summary>
    public string? AvatarUrl = string.Empty;

    /// <summary>
    /// Создание нового пользователя.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="name">Имя пользователя.</param>
    /// <param name="email">Электронная почта пользователя.</param>
    /// <param name="passwordHash">Хэш пароля пользователя.</param>
    /// <param name="roleId">Идентификатор роли пользователя.</param>
    /// <param name="avatarUrl">Путь к аватару пользователя.</param>
    /// <param name="country">Страна проживания.</param>
    /// <param name="city">Город проживания.</param>
    /// <param name="phone">Номер телефона.</param>
    /// <param name="telegram">Телеграм аккаунт.</param>
    /// <returns>Кортеж, содержащий созданного пользователя и сообщение об ошибке (если есть).</returns>
    public static (User user, string error) Create(Guid id, string name, string email, string passwordHash, Guid roleId, string avatarUrl, string country, string city, string phone, string? telegram)
    {
        var error = string.Empty;
        var user = new User(id, name, email, passwordHash, roleId, avatarUrl, country, city, phone, telegram);

        return (user, error);
    }

    /// <summary>
    /// Метод для регистрации.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="name">Имя пользователя.</param>
    /// <param name="email">Почта пользователя.</param>
    /// <param name="passwordHash">Хэш пароля.</param>
    /// <param name="roleId">Идентификатор роли.</param>
    /// <param name="avatarUrl">Путь аватарки.</param>
    /// <returns>Кортеж, содержащий созданного пользователя и сообщение об ошибке (если есть).</returns>
    public static (User user, string error) Register(Guid id, string name, string email, string passwordHash, Guid roleId, string avatarUrl)
    {
        var error = string.Empty;
        var user = new User(id, name, email, passwordHash, roleId, avatarUrl);
        
        return (user, error);
    }
}