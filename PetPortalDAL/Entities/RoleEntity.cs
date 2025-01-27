namespace PetPortalDAL.Entities;

/// <summary>
/// Role as data base model.
/// </summary>
public class RoleEntity
{
    /// <summary>
    /// Role identifier. 
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Role name.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Users.
    /// </summary>
    public ICollection<UserEntity> Users { get; set; }
}