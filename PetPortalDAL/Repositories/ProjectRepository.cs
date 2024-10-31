using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.DTOs;
using PetPortalCore.Models;

namespace PetPortalDAL.Repositories;

/// <summary>
/// Project repository.
/// </summary>
public class ProjectRepository : IProjectsRepository
{
    /// <summary>
    /// Mock data.
    /// </summary>
    private readonly List<Project> _mockProjects;
    
    /// <summary>
    /// Project repository constructor.
    /// </summary>
    public ProjectRepository()
    {
        _mockProjects = new List<Project>
        {
            Project.Create(Guid.NewGuid(), "Project 1", "Description for project 1", Guid.NewGuid()).project,
            Project.Create(Guid.NewGuid(), "Project 2", "Description for project 2", Guid.NewGuid()).project,
            Project.Create(Guid.NewGuid(), "Project 3", "Description for project 3", Guid.NewGuid()).project,
        };
    }
    
    /// <summary>
    /// Returns the list of mock projects
    /// </summary>
    public Task<List<Project>> Get()
    {
        return Task.FromResult(_mockProjects);
    }

    /// <summary>
    /// Simulates creating a new project and returns its ID
    /// </summary>
    public Task<Guid> Create(Project project)
    {
        _mockProjects.Add(project);
        return Task.FromResult(project.Id);
    }

    /// <summary>
    /// Simulates updating an existing project and returns its ID
    /// </summary>
    public Task<Guid> Update(ProjectDetailDto request)
    {
        var project = _mockProjects.Find(p => p.Id == request.Id);
        
        if (project != null)
        {
            project.Name = request.Name;        
            project.Description = request.Description;
            project.OwnerId = request.OwnerId;
        }
        
        return Task.FromResult(request.Id);
    }

    /// <summary>
    /// Simulates deleting a project by its ID and returns the ID
    /// </summary>
    public Task<Guid> Delete(Guid id)
    {
        var project = _mockProjects.Find(p => p.Id == id);
        
        if (project != null)
        {
            _mockProjects.Remove(project);
        }

        return Task.FromResult(id);
    }
}