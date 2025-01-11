namespace PetPortalCore.DTOs.Contracts;

/// <summary>
/// Member contract.
/// </summary>
public class MemberContract
{
    /// <summary>
    /// User identifier.
    /// </summary>
    public Guid UserId { get; set; } 
    
    /// <summary>
    /// Project identifier.
    /// </summary>
    public Guid ProjectId { get; set; }
}