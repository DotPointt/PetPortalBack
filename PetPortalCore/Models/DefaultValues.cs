namespace PetPortalCore.Models;

/// <summary>
/// Some default vlues.
/// </summary>
public static class DefaultValues
{
    /// <summary>
    /// Default new user role id.
    /// </summary>
    public static Guid RoleId { get; private set; } = Guid.Parse("00000000-0000-0000-0000-000000000002");
}