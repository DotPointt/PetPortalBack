using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.DTOs;
using PetPortalCore.Models.ProjectModels;
using PetPortalDAL.Entities;

namespace PetPortalDAL.Repositories;

/// <summary>
/// Project repository.
/// </summary>
public class ProjectsRepository : IProjectsRepository
{
    /// <summary>
    /// Data base context.
    /// </summary>
    private readonly PetPortalDbContext _context;
        
    /// <summary>
    /// Repository constructor.
    /// </summary>
    /// <param name="context">Data base context.</param>
    public ProjectsRepository(PetPortalDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get project paginated
    /// </summary>
    /// <returns>List of projects.</returns>
    public async Task<List<Project>> Get(int offset = 10, int page = 1)
    {
        var projectsEntities = await _context.Projects
            .AsNoTracking()
            .Skip((page - 1) * offset)
            .Take(offset)
            .ToListAsync();

        var projects = projectsEntities
            .Select(project =>
                Project.Create(project.Id,
                    project.Name, 
                    project.Description, 
                    project.OwnerId,
                    project.Deadline,
                    project.ApplyingDeadline,
                    project.StateOfProject
                    ).project)
            .ToList();

        return projects;
    }
    
    /// <summary>
    /// Gets all projects
    /// </summary>
    /// <returns></returns>
    public async Task<List<Project>> GetAll()
    {
        var projectsEntities = await _context.Projects
            .AsNoTracking()
            .ToListAsync();

        var projects = projectsEntities
            .Select(project =>
                Project.Create(project.Id,
                    project.Name, 
                    project.Description, 
                    project.OwnerId,
                    project.Deadline,
                    project.ApplyingDeadline,
                    project.StateOfProject
                ).project)
            .ToList();

        return projects;
    }

    /// <summary>
    /// Get project by identifier.
    /// </summary>
    /// <param name="projectId">Project identifier.</param>
    /// <returns>Project.</returns>
    public async Task<Project> GetById(Guid projectId)
    {
        var project = await _context.Projects
            .AsNoTracking()
            .Where(p => p.Id == projectId)
            .FirstOrDefaultAsync();
        
        if (project == null)
            throw new Exception("Project not found");
        
        return Project.Create(project.Id, project.Name, project.Description, project.OwnerId, project.Deadline, project.ApplyingDeadline, project.StateOfProject).project;
    }

    /// <summary>
    /// Create new project in database.
    /// </summary>
    /// <param name="projectData">Project data.</param>
    /// <returns>Created project identifier.</returns>
    public async Task<Guid> Create(Project projectData)
    {
        var projectEntity = new ProjectEntity()
        {
            Id = projectData.Id,
            Name = projectData.Name,
            Description = projectData.Description,
            OwnerId = projectData.OwnerId,
            Deadline = projectData.Deadline,
            ApplyingDeadline = projectData.ApplyingDeadline,
            StateOfProject = projectData.StateOfProject
        };

        await _context.AddAsync(projectEntity);
        await _context.SaveChangesAsync();

        return projectEntity.Id;
    }

    /// <summary>
    /// Update data base project.
    /// </summary>
    /// <param name="projectData">Project data.</param>
    /// <returns>Updated project identifier.</returns>
    public async Task<Guid> Update(ProjectDto projectData)
    {
        await _context.Projects
            .Where(project => project.Id == projectData.Id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(project => project.Name, project => projectData.Name)
                .SetProperty(project => project.Description, project => projectData.Description)
            );

        return projectData.Id;
    }

    /// <summary>
    /// Delete database project.
    /// </summary>
    /// <param name="id">Project identifier.</param>
    /// <returns>Deleted project identifier.</returns>
    public async Task<Guid> Delete(Guid id)
    {
        await _context.Projects
            .Where(project => project.Id == id)
            .ExecuteDeleteAsync();

        return id;
    }

    public async Task<int> GetProjectCountByOwnerIdAsync(Guid OwnerId)
    {
        return await _context.Projects
            .CountAsync(p => p.OwnerId == OwnerId);
    }
}