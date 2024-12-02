namespace PetPortalDAL.Entities;

/// <summary>
/// Project members.
/// </summary>
public class ProjectUser
{
    /// <summary>
    /// Link identifier.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Project identifier.
    /// </summary>
    public Guid ProjectId { get; set; }
    
    /// <summary>
    /// User identifier.
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Project entity.
    /// </summary>
    public ProjectEntity Project { get; set; }
    
    /// <summary>
    /// User entity.
    /// </summary>
    public UserEntity User { get; set; }
}