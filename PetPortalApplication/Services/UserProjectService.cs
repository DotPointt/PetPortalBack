using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.Models;

namespace PetPortalApplication.Services;

/// <summary>
/// Project memberships service.
/// </summary>
public class UserProjectService : IUserProjectService
{
    /// <summary>
    /// Project memberships repository.
    /// </summary>
    private readonly IUserProjectRepository _userProjectRepository; 
    
    /// <summary>
    /// Project memberships service constructor.
    /// </summary>
    /// <param name="userProjectRepository"></param>
    public UserProjectService(IUserProjectRepository userProjectRepository)
    {
        _userProjectRepository = userProjectRepository;
    }

    /// <summary>
    /// Get project members by project id.
    /// </summary>
    /// <param name="projectId">Project identifier.</param>
    /// <returns>List of project members.</returns>
    public async Task<List<User>> GetProjectMembers(Guid projectId)
    {
        return await _userProjectRepository.GetProjectMembers(projectId);
    }

    /// <summary>
    /// Add new member to project.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="projectId">Project identifier.</param>
    /// <returns>Guid of new member.</returns>
    public async Task<Guid> AddProjectMember(Guid userId, Guid projectId)
    {
        var guid = Guid.NewGuid();
        return await _userProjectRepository.AddProjectMember(userId, projectId, guid);
    }

    /// <summary>
    /// Delete member from project.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="projectId">Project identifier.</param>
    /// <returns>Deleted member identifier.</returns>
    public async Task<Guid> DeleteProjectMember(Guid userId, Guid projectId)
    {
        var members = await _userProjectRepository.GetProjectMembers(projectId);
        var memberId = members
            .Where(m => m.Id == userId)
            .Select(m => m.Id)
            .FirstOrDefault();
        
        return await _userProjectRepository.DeleteProjectMember(memberId);
    }
}