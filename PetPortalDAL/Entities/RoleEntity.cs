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
    /// Список пользователей, связанных с этой ролью.
    /// </summary>
    public ICollection<UserEntity> Users { get; set; }
}