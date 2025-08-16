namespace PetPortalCore.DTOs;

/// <summary>
/// DTO (Data Transfer Object) для отклика.
/// </summary>
public class RespondDto
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
}