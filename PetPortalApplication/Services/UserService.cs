using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.DTOs;
using PetPortalCore.DTOs.Contracts;
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

    private readonly IJwtProvider _jwtProvider;
    
    /// <summary>
    /// User service constructor.
    /// </summary>
    /// <param name="usersRepository"></param>
    public UserService(IUsersRepository usersRepository, IJwtProvider jwtProvider)
    {
        _usersRepository = usersRepository;
        _jwtProvider = jwtProvider;
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
    /// <param name="request">User data.</param>
    /// <returns>Created user guid.</returns>
    /// <exception cref="ArgumentException">Some parameters invalided.</exception>
    public async Task<Guid> Register(UserContract request)
    {
        
        var hashedPassword = request.Password;
        
        var (user, error) = PetPortalCore.Models.User.Create(
            Guid.NewGuid(),
            request.Name,
            request.Email,
            hashedPassword,
            request.RoleId);
                
        if (!string.IsNullOrEmpty(error))
        {
            throw new ArgumentException(error);
        }
        
        return await _usersRepository.Create(user);
    }


    public async Task<string> Login(string email, string password)
    {
        var user = await _usersRepository.GetByEmail(email);

        if (user == null)
            throw new Exception("No user registered");

        var result = true;  // ВЕРИФИКАЦИЯ var result = _passwordHasher.Verify(password, hashedPassword);

        if (result == false)
            throw new Exception("failed to login");

        var token = _jwtProvider.GenerateToken(user.Email);
        
        return token;
    }


    /// <summary>
    /// User updating.
    /// </summary>
    /// <param name="user">User data.</param>
    /// <returns>Updated user guid.</returns>
    public async Task<Guid> Update(UserDto user)
    {
        return await _usersRepository.Update(user);
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