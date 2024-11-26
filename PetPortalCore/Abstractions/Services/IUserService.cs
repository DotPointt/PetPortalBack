using PetPortalCore.DTOs;
using PetPortalCore.DTOs.Contracts;
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
    Task<Guid> Create(UserContract user);

    /// <summary>
    /// User updating.
    /// </summary>
    /// <param name="user">User data.</param>
    /// <returns>Updated user guid.</returns>
    Task<Guid> Update(UserDto user);

    /// <summary>
    /// User deleting.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <returns>Deleted user guid.</returns>
    Task<Guid> Delete(Guid id);
}