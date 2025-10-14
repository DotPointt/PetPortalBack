using PetPortalDAL.Entities.LinkingTables;

namespace PetPortalDAL.Entities;

/// <summary>
/// Сущность роли в базе данных.
/// </summary>
public class RoleEntity
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
    
    /// <summary>
    /// Список пользователей, связанных с этой ролью.
    /// </summary>
    public ICollection<UserEntity> Users { get; set; }

    public ICollection<ProjectRole> ProjectRoles { get; set; }
}