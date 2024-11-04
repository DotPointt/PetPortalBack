using PetPortalCore.DTOs;
using PetPortalCore.Models;

namespace PetPortalCore.Abstractions.Repositories
{
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
        /// User updating.
        /// </summary>
        /// <param name="userData">User data.</param>
        /// <returns>Identifier of updated user.</returns>
        Task<Guid> Update(UserDto userData);
        
        /// <summary>
        /// Delete user.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <returns>Identifier of deleted user.</returns>
        Task<Guid> Delete(Guid id);
    }
}