namespace PetPortalCore.DTOs;

/// <summary>
/// DTO (Data Transfer Object) для пользователя.
/// </summary>
public class UserDto
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid Id { get; set; }
        
    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public string Name { get; set; }
    
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
        
    /// <summary>
    /// Электронная почта пользователя.
    /// </summary>
    public string Email { get; set; }
        
    /// <summary>
    /// Пароль пользователя.
    /// </summary>
    public string Password { get; set; }
    
    public string PasswordHash { get; set; }
    
    /// <summary>
    /// Путь к файлу аватара пользователя.
    /// </summary>
    public string AvatarUrl { get; set; }
    
    /// <summary>
    /// Идентификатор роли пользователя.
    /// </summary>
    public Guid RoleId { get; set; }
}