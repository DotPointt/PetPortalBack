namespace PetPortalCore.DTOs.Contracts;

/// <summary>
/// User create contract.
/// </summary>
public class UserContract
{
    /// <summary>
    /// User name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// User email.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// User password.
    /// </summary>
    public string Password { get; set; }
    
    /// <summary>
    /// Role identifier.
    /// </summary>
    public Guid RoleId { get; set; }
}