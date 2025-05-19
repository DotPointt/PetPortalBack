namespace PetPortalCore.DTOs;

/// <summary>
/// Сокращенное дто пользователя.
/// </summary>
public class UserShortDto
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public string Name { get; set; } = null!;
    
    /// <summary>
    /// Ссылка на аватар.
    /// </summary>
    public string? AvatarUrl { get; set; }
}
