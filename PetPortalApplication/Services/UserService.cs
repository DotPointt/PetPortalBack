using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.Models;

namespace PetPortalApplication.Services;

/// <summary>
/// User service.
/// </summary>
public class UserService : IUserService
{
    /// <summary>
    /// User repository.
    /// </summary>
    private readonly IUsersRepository _usersRepository; 
    
    /// <summary>
    /// User service constructor.
    /// </summary>
    /// <param name="usersRepository"></param>
    public UserService(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }
    
    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns>List of users.</returns>
    public async Task<List<User>> GetAll()
    {
        return await _usersRepository.GetAll();
    }

    /// <summary>
    /// User creation.
    /// </summary>
    /// <param name="user">User data.</param>
    /// <returns>Created user guid.</returns>
    public async Task<Guid> Create(User user)
    {
        return await _usersRepository.Create(user);
    }

    /// <summary>
    /// User updating.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <param name="name">User name.</param>
    /// <param name="email">User email.</param>
    /// <param name="password">User password.</param>
    /// <returns>Updated user guid.</returns>
    public async Task<Guid> Update(Guid id, string name, string email, string password)
    {
        return await _usersRepository.Update(id, name, email, password);
    }

    /// <summary>
    /// User deleting.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <returns>Deleted user guid.</returns>
    public async Task<Guid> Delete(Guid id)
    {
        return await _usersRepository.Delete(id);
    }
}