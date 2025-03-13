namespace PetPortalCore.Contracts;

/// <summary>
/// Контракт для работы с участниками проекта.
/// </summary>
public class MemberContract
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; set; } 
    
    /// <summary>
    /// Идентификатор проекта.
    /// </summary>
    public Guid ProjectId { get; set; }
}