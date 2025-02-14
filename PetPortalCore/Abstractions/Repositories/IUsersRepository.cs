using PetPortalCore.DTOs;
using PetPortalCore.Models;

namespace PetPortalCore.Abstractions.Repositories;

/// <summary>
/// User service interface.
/// </summary>
public interface IUsersRepository
{
    /// <summary>
    /// Get all users.
    /// </summary>
    /// <returns>List of users.</returns>
    Task<List<User>> GetAll();
        
    /// <summary>
    /// Create new user.
    /// </summary>
    /// <param name="user">User data.</param>
    /// <returns>Identifier of new user.</returns>
    Task<Guid> Create(User user);

    /// <summary>
    /// Get user by email.
    /// </summary>
    /// <param name="email">User email.</param>
    /// <returns>User.</returns>
    public Task<User> GetByEmail(string email);
    
    /// <summary>
    /// Get user by identifier.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <returns>User.</returns>
    public Task<User> GetById(Guid userId);

    /// <summary>
    /// User updating.
    /// </summary>
    /// <param name="userData"></param>
    /// <returns>Identifier of updated user.</returns>
    Task<Guid> Update(UserDto userData);
        
    /// <summary>
    /// Delete user.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <returns>Identifier of deleted user.</returns>
    Task<Guid> Delete(Guid id);
}