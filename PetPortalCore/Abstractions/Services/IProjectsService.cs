using System.Diagnostics;
using PetPortalCore.DTOs;
using PetPortalCore.DTOs.Contracts;
using PetPortalCore.Models.ProjectModels;

namespace PetPortalCore.Abstractions.Services;

/// <summary>
/// Project service interface.
/// </summary>
public interface IProjectsService
{
    /// <summary>
    /// Get all projects.
    /// </summary>
    /// <returns>List of projects.</returns>
    Task<List<Project>> GetAll();
    
    /// <summary>
    /// Get project by id.
    /// </summary>
    /// <param name="id">Project identifier.</param>
    /// <returns>Project.</returns>
    Task<Project> GetById(Guid id);

    /// <summary>
    /// Get Projects paginated
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    public Task<List<Project>> GetPaginated(int offset = 10, int page = 1);
        
    /// <summary>
    /// Create new project.
    /// </summary>
    /// <param name="project">Project data.</param>
    /// <returns>Identifier of new project.</returns>
    Task<Guid> Create(ProjectContract project);

    /// <summary>
    /// Project updating.
    /// </summary>
    /// <param name="project">Project data.</param>
    /// <returns>Identifier of updated project.</returns>
    Task<Guid> Update(ProjectDto project);
        
    /// <summary>
    /// Delete project.
    /// </summary>
    /// <param name="id">Project identifier.</param>
    /// <returns>Identifier of deleted project.</returns>
    Task<Guid> Delete(Guid id);

    /// <summary>
    /// Fasle - ok no limit violation, True - too many projects. basic anti ddos check, can be implemented in raw sql
    /// </summary>
    Task<bool> CheckCreatingLimit(Guid OwnerId, int limit);
}