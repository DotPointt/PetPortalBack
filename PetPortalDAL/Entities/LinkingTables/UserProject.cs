namespace PetPortalDAL.Entities.LinkingTables;

/// <summary>
/// Связующая сущность между пользователями и проектами.
/// </summary>
public class UserProject
{
    /// <summary>
    /// Идентификатор связи.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Идентификатор проекта.
    /// </summary>
    public Guid ProjectId { get; set; }
    
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Ссылка на сущность проекта.
    /// </summary>
    public ProjectEntity Project { get; set; }
    
    /// <summary>
    /// Ссылка на сущность пользователя.
    /// </summary>
    public UserEntity User { get; set; }
}