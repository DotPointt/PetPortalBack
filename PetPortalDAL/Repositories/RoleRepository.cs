using Microsoft.EntityFrameworkCore;
using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.Models;

namespace PetPortalDAL.Repositories;

/// <summary>
/// Role repository.
/// </summary>
public class RoleRepository : IRoleRepository
{
    /// <summary>
    /// Data base context.
    /// </summary>
    private readonly PetPortalDbContext _context;
    
    /// <summary>
    /// Users repository.
    /// </summary>
    private readonly IUsersRepository _usersRepository;

    /// <summary>
    /// Repository constructor.
    /// </summary>
    /// <param name="context">Data base context.</param>
    /// <param name="usersRepository">Users repository.</param>
    public RoleRepository(PetPortalDbContext context, IUsersRepository usersRepository)
    {
        _context = context;
        _usersRepository = usersRepository;
    }

    /// <summary>
    /// get user role name.
    /// </summary>
    /// <param name="userId">user id.</param>
    /// <returns>Role name.</returns>
    public async Task<string> GetRoleByUserId(Guid userId)
    {
        var user = await _usersRepository.GetById(userId);
        
        var roleName = await _context.Roles
            .Where(role => role.Id == user.RoleId)
            .Select(role => role.Name)
            .FirstOrDefaultAsync();
        
        if (roleName is null)
            throw new KeyNotFoundException();
        
        return roleName;
    }
}