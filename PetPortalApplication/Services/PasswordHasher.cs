using Microsoft.Win32;
using PetPortalCore.Abstractions.Services;

namespace PetPortalApplication.Services;

/// <summary>
/// Password hasher.
/// </summary>
public class PasswordHasher : IPasswordHasher
{
    /// <summary>
    /// Hash password.
    /// </summary>
    /// <param name="password">Password.</param>
    /// <returns>Hash on password.</returns>
    public string HashPassword(string password) 
        => BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    
    /// <summary>
    /// Verify hashed password.
    /// </summary>
    /// <param name="hashedPassword">Hashed password.</param>
    /// <param name="providedPassword">Original password.</param>
    /// <returns>
    /// True - when hashed password is original;
    /// False - when is not.
    /// </returns>
    public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
        => BCrypt.Net.BCrypt.EnhancedVerify(providedPassword, hashedPassword);
}