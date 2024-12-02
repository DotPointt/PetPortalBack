using Microsoft.EntityFrameworkCore;
using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.Models;

namespace PetPortalDAL.Repositories;

/// <summary>
/// Projects memberships repository.
/// </summary>
public class UserProjectRepository : IUserProjectRepository
{
    /// <summary>
    /// Data base context.
    /// </summary>
    private readonly PetPortalDbContext _context;
    
    /// <summary>
    /// Repository constructor.
    /// </summary>
    /// <param name="context">Data base context.</param>
    public UserProjectRepository(PetPortalDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get project members bu project id.
    /// </summary>
    /// <param name="projectId">Project identifier.</param>
    /// <returns>List of project members.</returns>
    public async Task<List<User>> GetProjectMembers(Guid projectId)
    {
        var projectMemberIds = await _context.UserProjects
            .AsNoTracking()
            .Where(e => e.ProjectId == projectId)
            .Select(e => e.UserId)
            .ToListAsync();
        
        var users = await _context.Users
            .AsNoTracking()
            .Where(e => projectMemberIds.Contains(e.Id))
            .Select(user =>
                User.Create(user.Id, user.Name, user.Email, user.PasswordHash).user)
            .ToListAsync();

        return users;
    }
    
    // TODO Add and Delete methods.
}