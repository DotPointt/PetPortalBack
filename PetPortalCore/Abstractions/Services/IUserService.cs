using PetPortalCore.DTOs;
using PetPortalCore.DTOs.Contracts;
using PetPortalCore.Models;
using PetPortalCore.Models.ProjectModels;

namespace PetPortalCore.Abstractions.Services;

public interface IUserService
{
    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns>List of users.</returns>
    Task<List<User>> GetAll();

    /// <summary>
    /// Get projects by owner.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <returns>List of projects.</returns>
    Task<List<Project>> GetOwnProjects(Guid userId);
    
    /// <summary>
    /// User creation.
    /// </summary>
    /// <param name="user">User data.</param>
    /// <returns>Created user guid.</returns>
    Task<Guid> Register(UserContract user);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    Task<string> Login(string email, string password);

    /// <summary>
    /// User updating.
    /// </summary>
    /// <param name="userData">User updated data.</param>
    /// <returns>Updated user guid.</returns>
    Task<Guid> Update(UserDto userData);

    /// <summary>
    /// User avatar update.
    /// </summary>
    /// <param name="userData">User updated data.</param>
    /// <returns>Updated user guid.</returns>
    Task<Guid> UpdateAvatar(UserDto userData);

    /// <summary>
    /// User deleting.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <returns>Deleted user guid.</returns>
    Task<Guid> Delete(Guid id);

    /// <summary>
    ///  Gets user object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<User> GetUserById(Guid id);
}