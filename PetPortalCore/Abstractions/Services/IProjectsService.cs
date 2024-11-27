using System.Diagnostics;
using PetPortalCore.DTOs;
using PetPortalCore.DTOs.Contracts;
using PetPortalCore.Models;

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
}