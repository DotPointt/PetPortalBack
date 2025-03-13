using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.DTOs;
using PetPortalCore.Models;
using PetPortalDAL.Entities;


namespace PetPortalDAL.Repositories;

/// <summary>
/// Репозиторий для работы с проектами.
/// </summary>
public class ProjectsRepository : IProjectsRepository
{
    /// <summary>
    /// Контекст базы данных.
    /// </summary>
    private readonly PetPortalDbContext _context;
        
    /// <summary>
    /// Конструктор репозитория.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    public ProjectsRepository(PetPortalDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Получить проекты с пагинацией.
    /// </summary>
    /// <param name="sortOrder">Очередь сортировки.</param>
    /// <param name="sortItem">Элемент сортировки.</param>  
    /// <param name="offset">Количество проектов на странице.</param>
    /// <param name="page">Номер страницы.</param>
    /// <returns>Список проектов.</returns>
    public async Task<List<Project>> Get( string? sortItem, string? sortOrder,  int offset = 10, int page = 1)
    {
        var projectsQuery = _context.Projects
            .AsNoTracking()
            .Skip((page - 1) * offset)
            .Take(offset);

        Expression<Func<ProjectEntity, object>> selectorKey = sortItem?.ToLower() switch
        {
            "date" => project => project.CreatedDate,
            "name" => project => project.Name,
            "applyingdeadline" => project => project.ApplyingDeadline,
            _ => project => project.Id
        };

        projectsQuery = sortOrder == "desc"
            ? projectsQuery.OrderByDescending(selectorKey)
            : projectsQuery.OrderBy(selectorKey);

        var projectsEntities = await projectsQuery.ToListAsync();
        
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
    /// Получить проекты, созданные определенным пользователем.
    /// </summary>
    /// <param name="ownerId">Идентификатор пользователя.</param>
    /// <returns>Список проектов.</returns>
    public async Task<List<Project>> GetOwnProjects(Guid ownerId)
    {
        var projectsEntities = await _context.Projects
            .AsNoTracking()
            .Where(project => project.OwnerId == ownerId)
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
    /// Получить все проекты.
    /// </summary>
    /// <returns>Список проектов.</returns>
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
    /// Получить проект по идентификатору.
    /// </summary>
    /// <param name="projectId">Идентификатор проекта.</param>
    /// <returns>Проект.</returns>
    /// <exception cref="Exception">Выбрасывается, если проект не найден.</exception>
    public async Task<Project> GetById(Guid projectId)
    {
        var project = await _context.Projects
            .AsNoTracking()
            .Where(p => p.Id == projectId)
            .FirstOrDefaultAsync();
        
        if (project == null)
            throw new Exception("Проект не найден.");
        
        return Project.Create(project.Id, project.Name, project.Description, project.OwnerId, project.Deadline, project.ApplyingDeadline, project.StateOfProject).project;
    }

    /// <summary>
    /// Создать новый проект в базе данных.
    /// </summary>
    /// <param name="projectData">Данные проекта.</param>
    /// <returns>Идентификатор созданного проекта.</returns>
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
    /// Обновить проект в базе данных.
    /// </summary>
    /// <param name="projectData">Данные проекта.</param>
    /// <returns>Идентификатор обновленного проекта.</returns>
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
    /// Удалить проект из базы данных.
    /// </summary>
    /// <param name="id">Идентификатор проекта.</param>
    /// <returns>Идентификатор удаленного проекта.</returns>
    public async Task<Guid> Delete(Guid id)
    {
        await _context.Projects
            .Where(project => project.Id == id)
            .ExecuteDeleteAsync();

        return id;
    }
    
    /// <summary>
    /// Получить количество проектов, созданных определенным пользователем.
    /// </summary>
    /// <param name="ownerId">Идентификатор пользователя.</param>
    /// <returns>Количество проектов.</returns>
    public async Task<int> GetProjectCountByOwnerIdAsync(Guid ownerId)
    {
        return await _context.Projects
            .AsNoTracking()
            .CountAsync(p => p.OwnerId == ownerId);
    }
}