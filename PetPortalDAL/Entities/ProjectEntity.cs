namespace PetPortalDAL.Entities;

/// <summary>
/// Project as data base model.
/// </summary>
public class ProjectEntity
{
    /// <summary>
    /// Project identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Project name.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Project description.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Project owner.
    /// </summary>
    public Guid OwnerId { get; set; }
        
    /// <summary>
    /// Owner.
    /// </summary>
    public UserEntity User { get; set; }
    
    /// <summary>
    /// Project members.
    /// </summary>
    public List<UserEntity> Members { get; set; }
}