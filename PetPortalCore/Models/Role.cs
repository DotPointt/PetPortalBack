namespace PetPortalCore.Models;

/// <summary>
/// Класс, представляющий роль пользователя.
/// </summary>
public class Role
{
    /// <summary>
    /// Идентификатор роли.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Название роли.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    ///  Флаг: системная роль нельзя удалять, например "Другое"
    /// </summary>
    public bool IsSystem { get; set; }
    
}