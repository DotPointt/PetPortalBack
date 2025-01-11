namespace PetPortalCore.DTOs;

/// <summary>
/// User dto.
/// </summary>
public class UserDto
{
    /// <summary>
    /// User identifier.
    /// </summary>
    public Guid Id { get; set; }
        
    /// <summary>
    /// User name.
    /// </summary>
    public string Name { get; set; }
        
    /// <summary>
    /// User email.
    /// </summary>
    public string Email { get; set; }
        
    /// <summary>
    /// User password
    /// </summary>
    public string Password { get; set; }
    
    /// <summary>
    /// Role identifier.
    /// </summary>
    public Guid RoleId { get; set; }
}