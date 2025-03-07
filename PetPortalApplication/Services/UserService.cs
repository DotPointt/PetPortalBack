using Microsoft.AspNetCore.Identity;
using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.DTOs;
using PetPortalCore.DTOs.Contracts;
using PetPortalCore.Models;
using PetPortalDAL.Repositories;

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
    /// Role reporitoey.
    /// </summary>
    private readonly IRoleRepository _roleRepository;

    /// <summary>
    /// Auth provider.
    /// </summary>
    private readonly IJwtProvider _jwtProvider;
    
    /// <summary>
    /// Password hasher.
    /// </summary>
    private readonly IPasswordHasher _passwordHasher;

    /// <summary>
    /// User service constructor.
    /// </summary>
    /// <param name="usersRepository">Users repository.</param>
    /// <param name="jwtProvider">Auth provider.</param>
    /// <param name="passwordHasher">Password hasher.</param>
    /// <param name="roleRepository">Role repository.</param>
    public UserService(IUsersRepository usersRepository, IJwtProvider jwtProvider, IPasswordHasher passwordHasher, IRoleRepository roleRepository)
    {
        _usersRepository = usersRepository;
        _jwtProvider = jwtProvider;
        _passwordHasher = passwordHasher;
        _roleRepository = roleRepository;
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
        var hashedPassword = _passwordHasher.HashPassword(request.Password);
        
        var (user, error) = PetPortalCore.Models.User.Create(
            Guid.NewGuid(),
            request.Name,
            request.Email,
            hashedPassword,
            request.RoleId,
            string.Empty); //TODO Указать путь к дэфолтной автарке.
                
        if (!string.IsNullOrEmpty(error))
        {
            throw new ArgumentException(error);
        }
        
        return await _usersRepository.Create(user);
    }
    
    /// <summary>
    /// User login.
    /// </summary>
    /// <param name="email">User email.</param>
    /// <param name="password">User password.</param>
    /// <returns>Jwt token.</returns>
    /// <exception cref="Exception"></exception>
    public async Task<string> Login(string email, string password)
    {
        var user = await _usersRepository.GetByEmail(email);

        if (user == null)
            throw new Exception("No user registered");

        var verify = _passwordHasher.VerifyHashedPassword(user.PasswordHash, password);

        if (!verify)
            throw new Exception("failed to login");

        var roleName = await _roleRepository.GetRoleByUserId(user.Id);
        
        var token = _jwtProvider.GenerateToken(user.Id, user.Email, roleName);
        
        return token;
    }
    
    /// <summary>
    /// User updating.
    /// </summary>
    /// <param name="userData">User updated data.</param>
    /// <returns>Updated user guid.</returns>
    public async Task<Guid> Update(UserDto userData)
    {
        return await _usersRepository.Update(userData);
    }

    /// <summary>
    /// User avatar update.
    /// </summary>
    /// <param name="userData">User updated data.</param>
    /// <returns>Updated user guid.</returns>
    public async Task<Guid> UpdateAvatar(UserDto userData)
    {
        var user = await _usersRepository.GetById(userData.Id);
        
        var fullUserData = new UserDto()
        {
            Id = user.Id,            
            Name = user.Name,
            Email = user.Email,
            Password = user.PasswordHash,
            AvatarUrl = userData.AvatarUrl,
            RoleId = user.RoleId,
        };
        
        return await _usersRepository.Update(fullUserData);
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

    /// <summary>
    /// Gets user object by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<User> GetUserById(Guid id)
    {
        return await _usersRepository.GetById(id);
    }
}