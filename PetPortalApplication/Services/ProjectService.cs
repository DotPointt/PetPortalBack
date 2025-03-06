using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.DTOs;
using PetPortalCore.DTOs.Contracts;
using PetPortalCore.Models.ProjectModels;

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
        return await _projectsRepository.GetAll();
    }

    /// <summary>
    /// Get Projects paginated
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    public async Task<List<Project>> GetPaginated(int offset = 10, int page = 1)
    {
        return await _projectsRepository.Get(offset, page );
    }

    /// <summary>
    /// Project creation.
    /// </summary>
    /// <param name="request">Project detail data.</param>
    /// <returns>Created project guid.</returns>
    /// <exception cref="ArgumentException">Some parameters invalided.</exception>
    public async Task<Guid> Create(ProjectContract request)
    {

        var (project, error) = Project.Create(
            Guid.NewGuid(),
            request.Name,
            request.Description,
            request.OwnerId,
            request.Deadline,
            request.ApplyingDeadline,
            request.StateOfProject);
        
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
    public async Task<Guid> Update(ProjectDto project)
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


    /// <summary>
    /// Fasle - ok no limit violation, True - too many projects. basic anti ddos check, can be implemented in raw sql
    /// </summary>
    /// <param name="OwnerId"></param>
    /// <param name="limit"></param>
    /// <returns></returns>
    public async Task<bool> CheckCreatingLimit(Guid OwnerId, int limit)
    {
        var cnt = await _projectsRepository.GetProjectCountByOwnerIdAsync(OwnerId);

        return cnt <= limit;
    }
}
