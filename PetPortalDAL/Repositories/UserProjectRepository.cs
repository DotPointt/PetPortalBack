using Microsoft.EntityFrameworkCore;
using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.Models;
using PetPortalDAL.Entities;

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
                User.Create(user.Id, user.Name, user.Email, user.PasswordHash, user.RoleId).user)
            .ToListAsync();

        return users;
    }

    /// <summary>
    /// Add new member to project.
    /// </summary>
    /// <param name="memberId">Member identifier.</param>
    /// <param name="projectId">Project identifier.</param>
    /// <param name="userId">User identifier.</param>
    /// <returns>Guid of new member.</returns>
    public async Task<Guid> AddProjectMember(Guid memberId, Guid projectId, Guid userId)
    {
        var newProjectMember = new UserProject()
        {
            Id = memberId,
            ProjectId = projectId,
            UserId = userId
        };
        
        await _context.UserProjects.AddAsync(newProjectMember);
        await _context.SaveChangesAsync();
        
        return newProjectMember.Id;
    }

    /// <summary>
    /// Delete member from database.
    /// </summary>
    /// <param name="memberId">Member identifier.</param>
    /// <returns>Deleted member identifier.</returns>
    public async Task<Guid> DeleteProjectMember(Guid memberId)
    {
        await _context.UserProjects
            .Where(m => m.Id == memberId)
            .ExecuteDeleteAsync();
        
        await _context.SaveChangesAsync();
        return memberId;
    }
}