using PetPortalCore.DTOs;
using PetPortalCore.Models.ProjectModels;

namespace PetPortalCore.Abstractions.Repositories;

/// <summary>
/// Project repository interface.
/// </summary>
public interface IProjectsRepository
{
    /// <summary>
    /// Get projects from db.
    /// </summary>
    /// <returns>List of projects.</returns>
    Task<List<Project>> Get( string? sortOrder, string? sortItem, int offset = 10, int page = 1);

    /// <summary>
    /// Gets all projects
    /// </summary>
    /// <returns></returns>
    public Task<List<Project>> GetAll();

    /// <summary>
    /// Get project by id.
    /// </summary>
    /// <param name="projectId">Project identifier.</param>
    /// <returns>Project.</returns>
    public Task<Project> GetById(Guid projectId);
    
    /// <summary>
    /// Create new project in db.
    /// </summary>
    /// <param name="project">Project data.</param>
    /// <returns>Project identifier.</returns>
    Task<Guid> Create(Project project);

    /// <summary>
    /// Update project in db.
    /// </summary>
    /// <param name="project">Project data.</param>
    /// <returns>Identifier of updated project.</returns>
    Task<Guid> Update(ProjectDto project);
        
    /// <summary>
    /// Delete project in db.
    /// </summary>
    /// <param name="id">Project identifier.</param>
    /// <returns>Identifier of deleted project.</returns>
    Task<Guid> Delete(Guid id);

    Task<int> GetProjectCountByOwnerIdAsync(Guid OwnerId);
}