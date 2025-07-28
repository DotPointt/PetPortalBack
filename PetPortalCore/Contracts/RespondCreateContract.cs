namespace PetPortalCore.Contracts;

/// <summary>
/// Контракт на создание отклика.
/// </summary>
public class RespondCreateContract
{   
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