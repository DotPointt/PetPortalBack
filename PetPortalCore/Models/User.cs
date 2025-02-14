using System.Runtime.InteropServices;

namespace PetPortalCore.Models;

/// <summary>
/// User.
/// </summary>
public class User
{
    /// <summary>
    /// User constructor.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <param name="name">User name.</param>
    /// <param name="email">User email.</param>
    /// <param name="passwordHash">User password.</param>
    /// <param name="roleId">Role identifier.</param>
    /// <param name="avatarUrl"></param>
    public User(Guid id, string name, string email, string passwordHash, Guid roleId, string avatarUrl)
    {
        Id = id;
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        RoleId = roleId;
        AvatarUrl = avatarUrl ?? string.Empty; 
    }
        
    /// <summary>
    /// User identifier.
    /// </summary>
    public Guid Id;

    /// <summary>
    /// User name.
    /// </summary>
    public string Name = string.Empty;

    /// <summary>
    /// User email.
    /// </summary>
    public string Email = string.Empty;

    /// <summary>
    /// User password.
    /// </summary>
    public string PasswordHash = string.Empty;
    
    /// <summary>
    /// Role identifier.
    /// </summary>
    public Guid RoleId = Guid.Empty;
    
    /// <summary>
    /// Avatar photo path.
    /// </summary>
    public string AvatarUrl = string.Empty;

    /// <summary>
    /// Creation new user.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <param name="name">User name.</param>
    /// <param name="email">User email.</param>
    /// <param name="passwordHash">User password.</param>
    /// <param name="roleId">Role identifier.</param>
    /// <param name="avatarUrl">Avatar photo path</param>
    /// <returns>(user, error if it exists)</returns>
    public static (User user, string error) Create(Guid id, string name, string email, string passwordHash, Guid roleId, string avatarUrl)
    {
        var error = string.Empty;
        var user = new User(id, name, email, passwordHash, roleId, avatarUrl);

        return (user, error);
    }
}