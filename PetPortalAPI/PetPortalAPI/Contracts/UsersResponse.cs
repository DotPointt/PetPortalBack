namespace PetPortalAPI.Contracts
{
    /// <summary>
    /// User data.
    /// </summary>
    /// <param name="Id">User identifier.</param>
    /// <param name="Name">User name.</param>
    /// <param name="Email">User email.</param>
    /// <param name="Password">User password.</param>
    public record UsersResponse
    (
        Guid Id,
        string Name,
        string Email,
        string Password
    );
}