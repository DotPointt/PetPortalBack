using PetPortalCore.Models;

namespace PetPortalCore.Abstractions.Repositories;

/// <summary>
/// Project memberships repository interface.
/// </summary>
public interface IUserProjectRepository
{
    /// <summary>
    /// Get project members bu project id.
    /// </summary>
    /// <param name="projectId">Project identifier.</param>
    /// <returns>List of project members.</returns>
    Task<List<User>> GetProjectMembers(Guid projectId);
}