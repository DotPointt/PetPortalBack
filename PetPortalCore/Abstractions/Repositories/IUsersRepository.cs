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
        /// <param name="project">User data.</param>
        /// <returns>Identifier of new user.</returns>
        Task<Guid> Create(User project);

        /// <summary>
        /// User updating.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <param name="name">User name.</param>
        /// <param name="email">User email.</param>
        /// <param name="password">User password.</param>
        /// <returns>Identifier of updated user.</returns>
        Task<Guid> Update(Guid id, string name, string email, string password);
        
        /// <summary>
        /// Delete user.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <returns>Identifier of deleted user.</returns>
        Task<Guid> Delete(Guid id);
    }
}