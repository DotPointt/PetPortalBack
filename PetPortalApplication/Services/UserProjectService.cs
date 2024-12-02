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
    /// Get project members bu project id.
    /// </summary>
    /// <param name="projectId">Project identifier.</param>
    /// <returns>List of project members.</returns>
    public async Task<List<User>> GetProjectMembers(Guid projectId)
    {
        // TODO validation.
        return await _userProjectRepository.GetProjectMembers(projectId);
    }
    
    // TODO Add and Delete methods.
}