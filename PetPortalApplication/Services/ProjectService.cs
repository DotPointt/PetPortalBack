using PetPortalCore.Abstractions;
using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.Models;

namespace PetPortalApplication.Services;

/// <summary>
/// Projects service.
/// </summary>
public class ProjectService() :  IProjectsService
{
    /// <summary>
    /// Project repository.
    /// </summary>
    private readonly IProjectsRepository projectsRepository; 
        
    /// <summary>
    /// Get all projects
    /// </summary>
    /// <returns>List of projects.</returns>
    public async Task<List<Project>> GetAllProjects()
    {
        return await projectsRepository.Get();
    }

    /// <summary>
    /// Project creation.
    /// </summary>
    /// <param name="project">Project data.</param>
    /// <returns>Created project guid.</returns>
    public async Task<Guid> CreateProject(Project project)
    {
        return await projectsRepository.Create(project);
    }

    /// <summary>
    /// Project updating.
    /// </summary>
    /// <param name="id">Project identifier.</param>
    /// <param name="name">Project name.</param>
    /// <param name="description">Project description.</param>
    /// <returns>Updated project guid.</returns>
    public async Task<Guid> UpdateProject(Guid id, string name, string description)
    {
        return await projectsRepository.Update(id, name, description);
    }

    /// <summary>
    /// Project deleting.
    /// </summary>
    /// <param name="id">Project identifier.</param>
    /// <returns>Deleted project guid.</returns>
    public async Task<Guid> Delete(Guid id)
    {
        return await projectsRepository.Delete(id);
    }
}