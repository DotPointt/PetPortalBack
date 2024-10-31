using PetPortalCore.Abstractions;
using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.DTOs;
using PetPortalCore.Models;

namespace PetPortalApplication.Services;

/// <summary>
/// Projects service.
/// </summary>
public class ProjectService :  IProjectsService
{
    /// <summary>
    /// Project repository.
    /// </summary>
    private readonly IProjectsRepository _projectsRepository; 
    
    /// <summary>
    /// Project service constructor.
    /// </summary>
    /// <param name="projectsRepository"></param>
    public ProjectService(IProjectsRepository projectsRepository)
    {
        _projectsRepository = projectsRepository;
    }
    
    /// <summary>
    /// Get all projects
    /// </summary>
    /// <returns>List of projects.</returns>
    public async Task<List<Project>> GetAll()
    {
        return await _projectsRepository.Get();
    }

    /// <summary>
    /// Project creation.
    /// </summary>
    /// <param name="request">Project detail data.</param>
    /// <returns>Created project guid.</returns>
    /// <exception cref="ArgumentException">Some parameters invalided.</exception>
    public async Task<Guid> Create(ProjectDetailDto request)
    {
        var (project, error) = Project.Create(
            Guid.NewGuid(),
            request.Name,
            request.Description,
            request.OwnerId);
        
        if (!string.IsNullOrEmpty(error))
        {
            throw new ArgumentException(error);
        }
        
        return await _projectsRepository.Create(project);
    }

    /// <summary>
    /// Project updating.
    /// </summary>
    /// <param name="project">Project data.</param>
    /// <returns>Updated project guid.</returns>
    public async Task<Guid> Update(ProjectDetailDto project)
    {
        return await _projectsRepository.Update(project);
    }

    /// <summary>
    /// Project deleting.
    /// </summary>
    /// <param name="id">Project identifier.</param>
    /// <returns>Deleted project guid.</returns>
    public async Task<Guid> Delete(Guid id)
    {
        return await _projectsRepository.Delete(id);
    }
}