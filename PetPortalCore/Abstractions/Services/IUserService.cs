using PetPortalCore.Models;

namespace PetPortalCore.Abstractions.Services;

public interface IUserService
{
    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns>List of users.</returns>
    Task<List<User>> GetAll();

    /// <summary>
    /// User creation.
    /// </summary>
    /// <param name="user">User data.</param>
    /// <returns>Created user guid.</returns>
    Task<Guid> Create(User user);

    /// <summary>
    /// User updating.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <param name="name">User name.</param>
    /// <param name="email">User email.</param>
    /// <param name="password">User password.</param>
    /// <returns>Updated user guid.</returns>
    Task<Guid> Update(Guid id, string name, string email, string password);

    /// <summary>
    /// User deleting.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <returns>Deleted user guid.</returns>
    Task<Guid> Delete(Guid id);
}