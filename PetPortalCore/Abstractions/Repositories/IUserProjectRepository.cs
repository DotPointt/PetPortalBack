using PetPortalCore.Models;

namespace PetPortalCore.Abstractions.Repositories;

/// <summary>
/// Project memberships repository interface.
/// </summary>
public interface IUserProjectRepository
{
    /// <summary>
    /// Get project members by project id.
    /// </summary>
    /// <param name="projectId">Project identifier.</param>
    /// <returns>List of project members.</returns>
    Task<List<User>> GetProjectMembers(Guid projectId);

    /// <summary>
    /// Add new member to project.
    /// </summary>
    /// <param name="memberId">Member identifier.</param>
    /// <param name="projectId">Project identifier.</param>
    /// <param name="userId">User identifier.</param>
    /// <returns>Guid of new member.</returns>
    Task<Guid> AddProjectMember(Guid memberId, Guid projectId, Guid userId);

    /// <summary>
    /// Delete member from database.
    /// </summary>
    /// <param name="memberId">Member identifier.</param>
    /// <returns>Deleted member identifier.</returns>
    Task<Guid> DeleteProjectMember(Guid memberId);
}