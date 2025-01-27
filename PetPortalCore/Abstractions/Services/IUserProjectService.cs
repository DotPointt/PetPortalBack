using PetPortalCore.Models;

namespace PetPortalCore.Abstractions.Services;

public interface IUserProjectService
{
    /// <summary>
    /// Get project members bu project id.
    /// </summary>
    /// <param name="projectId">Project identifier.</param>
    /// <returns>List of project members.</returns>
    Task<List<User>> GetProjectMembers(Guid projectId);

    /// <summary>
    /// Add new member to project.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="projectId">Project identifier.</param>
    /// <returns>Guid of new member.</returns>
    Task<Guid> AddProjectMember(Guid userId, Guid projectId);

    /// <summary>
    /// Delete member from project.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="projectId">Project identifier.</param>
    /// <returns>Deleted member identifier.</returns>
    Task<Guid> DeleteProjectMember(Guid userId, Guid projectId);
}