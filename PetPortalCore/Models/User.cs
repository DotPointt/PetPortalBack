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
    public User(Guid id, string name, string email, string passwordHash, Guid roleId, string avatarUrl)
    {
        Id = id;
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        RoleId = roleId;
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
    /// <returns>Кортеж, содержащий созданного пользователя и сообщение об ошибке (если есть).</returns>
    public static (User user, string error) Create(Guid id, string name, string email, string passwordHash, Guid roleId, string avatarUrl)
    {
        var error = string.Empty;
        var user = new User(id, name, email, passwordHash, roleId, avatarUrl);

        return (user, error);
    }
}