using PetPortalCore.Models;

namespace PetPortalDAL.Entities;

/// <summary>
/// Сущность отклика.
/// </summary>
public class RespondEntity
{
    /// <summary>
    /// Идентификатор отклика.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Желаемая роль на проекте.
    /// </summary>
    public string Role { get; set; }
    
    /// <summary>
    /// Комментарий отклика.
    /// </summary>
    public string Comment { get; set; }
    
    /// <summary>
    /// Идентификатор откликнувшегося.
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Идентификатор проекта.
    /// </summary>
    public Guid ProjectId { get; set; }
    
    /// <summary>
    /// Проект.
    /// </summary>
    public ProjectEntity Project { get; set; }
    
    /// <summary>
    /// Откликнувшийся.
    /// </summary>
    public UserEntity Responder { get; set; }
}