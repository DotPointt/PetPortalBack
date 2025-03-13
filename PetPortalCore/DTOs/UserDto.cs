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
    /// Электронная почта пользователя.
    /// </summary>
    public string Email { get; set; }
        
    /// <summary>
    /// Пароль пользователя.
    /// </summary>
    public string Password { get; set; }
    
    /// <summary>
    /// Путь к файлу аватара пользователя.
    /// </summary>
    public string AvatarUrl { get; set; }
    
    /// <summary>
    /// Идентификатор роли пользователя.
    /// </summary>
    public Guid RoleId { get; set; }
}