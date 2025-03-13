using PetPortalDAL.Entities.LinkingTables;

namespace PetPortalDAL.Entities;

/// <summary>
/// Сущность пользователя в базе данных.
/// </summary>
public class UserEntity
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
    /// Хэш пароля пользователя.
    /// </summary>
    public string PasswordHash { get; set; }
    
    /// <summary>
    /// Идентификатор роли пользователя.
    /// </summary>
    public Guid RoleId { get; set; }
    
    /// <summary>
    /// Путь к файлу аватара пользователя.
    /// </summary>
    public string? AvatarUrl { get; set; }
    
    /// <summary>
    /// Роль пользователя.
    /// </summary>
    public RoleEntity RoleEntity { get; set; }
    
    /// <summary>
    /// Список проектов, связанных с пользователем.
    /// </summary>
    public ICollection<UserProject> UserProjects { get; set; }
}