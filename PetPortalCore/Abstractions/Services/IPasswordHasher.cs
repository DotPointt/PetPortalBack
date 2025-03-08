namespace PetPortalCore.Abstractions.Services;

public interface IPasswordHasher
{
    /// <summary>
    /// Hash password.
    /// </summary>
    /// <param name="password">Password.</param>
    /// <returns>Hash on password.</returns>
    string HashPassword(string password);

    /// <summary>
    /// Verify hashed password.
    /// </summary>
    /// <param name="hashedPassword">Hashed password.</param>
    /// <param name="providedPassword">Original password.</param>
    /// <returns>
    /// True - when hashed password is original;
    /// False - when is not.
    /// </returns>
    bool VerifyHashedPassword(string hashedPassword, string providedPassword);
}