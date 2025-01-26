using Microsoft.EntityFrameworkCore;
using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.DTOs;
using PetPortalCore.Models;
using PetPortalDAL.Entities;

namespace PetPortalDAL.Repositories;

/// <summary>
/// Users repository.
/// </summary>
public class UsersRepository : IUsersRepository
{
    /// <summary>
    /// Data base context.
    /// </summary>
    private readonly PetPortalDbContext _context;
        
    /// <summary>
    /// Repository constructor.
    /// </summary>
    /// <param name="context">Data base context.</param>
    public UsersRepository(PetPortalDbContext context)
    {
        _context = context;
    }
        
    /// <summary>
    /// Get data bases users.
    /// </summary>
    /// <returns>List of users.</returns>
    public async Task<List<User>> GetAll()
    {
        var userEntities = await _context.Users
            .AsNoTracking()
            .ToListAsync();

        var users = userEntities
            .Select(user =>
                User.Create(user.Id, user.Name, user.Email, user.PasswordHash, user.RoleId).user)
            .ToList();

        return users;
    }

    /// <summary>
    /// Create new user in database.
    /// </summary>
    /// <param name="user">User data.</param>
    /// <returns>Created user identifier.</returns>
    public async Task<Guid> Create(User user)
    {
        var userEntity = new UserEntity()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            PasswordHash = user.PasswordHash,
            RoleId = user.RoleId
        };

        await _context.AddAsync(userEntity);
        await _context.SaveChangesAsync();

        return userEntity.Id;
    }

    /// <summary>
    /// Update data base user.
    /// </summary>
    /// <param name="userData">User data.</param>
    /// <returns>Updated user identifier.</returns>
    public async Task<Guid> Update(UserDto userData)
    {
        await _context.Users
            .Where(project => project.Id == userData.Id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(project => project.Name, project => userData.Name)
            );

        return userData.Id;
    }

    /// <summary>
    /// Delete data base user.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <returns>Deleted user identifier.</returns>
    public async Task<Guid> Delete(Guid id)
    {
        await _context.Users
            .Where(user => user.Id == id)
            .ExecuteDeleteAsync();

        return id;
    }
}