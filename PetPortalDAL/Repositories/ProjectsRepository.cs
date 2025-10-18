using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.DTOs;
using PetPortalCore.Models;
using PetPortalDAL.Entities;
using PetPortalDAL.Entities.LinkingTables;


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
    
    private readonly IRabbitMqProducerService _producerService;
        
    /// <summary>
    /// Конструктор репозитория.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    public ProjectsRepository(PetPortalDbContext context, IRabbitMqProducerService  producerService)
    {
        _context = context;
        _producerService =  producerService;
    }

    /// <summary>
    /// Получить проекты с пагинацией.
    /// </summary>
    /// <param name="sortOrder">Очередь сортировки.</param>
    /// <param name="sortItem">Элемент сортировки.</param>
    /// <param name="searchElement">Поиск.</param>
    /// <param name="offset">Количество проектов на странице.</param>`
    /// <param name="page">Номер страницы.</param>
    /// <returns>Список отсортированных проектов.</returns>
    public async Task<List<Project>> Get(bool sortOrder, string? sortItem, string searchElement, int offset = 10, int page = 1, ProjectFilterDTO filters = null)
    {
        var projectsQuery = _context.Projects
            .AsNoTracking();
        
        projectsQuery = ApplySearchFilter(projectsQuery, searchElement);
        projectsQuery = ApplyFilters(projectsQuery, filters);
        
        

        Expression<Func<ProjectEntity, object>> selectorKey = sortItem?.ToLower() switch
        {
            "date" => project => project.CreatedDate,
            "budget" => project => project.Budget,
            "applyingdeadline" => project => project.ApplyingDeadline,
            "deadline" => project => project.Deadline,
            _ => project => project.Id
        };

        projectsQuery = sortOrder   
            ? projectsQuery.OrderBy(selectorKey)
            : projectsQuery.OrderByDescending(selectorKey);
        
        
        var projectsEntities = await projectsQuery
            .Include(p => p.ProjectTags)
                .ThenInclude(pt => pt.Tag)
            .Include(p => p.ProjectRoles)
            .ThenInclude(Pr => Pr.Role)
            .Skip((page - 1) * offset)
            .Take(offset)
            .ToListAsync();
        
        var projects = projectsEntities
            .Select(project => new Project
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Requirements = project.Requirements,
                TeamDescription = project.TeamDescription,
                Plan = project.Plan,
                Result = project.Result,
                OwnerId = project.OwnerId,
                Deadline = project.Deadline,
                ApplyingDeadline = project.ApplyingDeadline,
                StateOfProject = project.StateOfProject,
                IsBusinesProject = project.IsBusinesProject,
                Budget = project.Budget,
                Tags = project.ProjectTags
                    .Select(pt => new Tag
                    {
                        Id = pt.Tag.Id,
                        Name = pt.Tag.Name
                    })
                    .ToList(),
                RequiredRoles = project.ProjectRoles
                    .Select(pr => new RequiredRole
                    {
                        RoleId = pr.Role.Id,
                        CustomRoleName = pr.CustomRoleName,
                        SystemRoleName = pr.Role.Name
                    })
                    .ToList()
            })
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
            .Select(project => new Project
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Requirements = project.Requirements,
                TeamDescription = project.TeamDescription,
                Plan = project.Plan,
                Result = project.Result,
                OwnerId = project.OwnerId,
                Deadline = project.Deadline,
                ApplyingDeadline = project.ApplyingDeadline,
                StateOfProject = project.StateOfProject,
                IsBusinesProject = project.IsBusinesProject,
                Budget = project.Budget
            })
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
            .Select(project => new Project
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Requirements = project.Requirements,
                TeamDescription = project.TeamDescription,
                Plan = project.Plan,
                Result = project.Result,
                OwnerId = project.OwnerId,
                Deadline = project.Deadline,
                ApplyingDeadline = project.ApplyingDeadline,
                StateOfProject = project.StateOfProject,
                IsBusinesProject = project.IsBusinesProject,
                Budget = project.Budget
            })
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
            .Include(p => p.ProjectRoles)
                .ThenInclude(pt => pt.Role)
            .AsNoTracking()
            .Where(p => p.Id == projectId)
            .FirstOrDefaultAsync();
        
        if (project == null)
            throw new Exception("Проект не найден.");

        return new Project
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            Requirements = project.Requirements,
            TeamDescription = project.TeamDescription,
            Plan = project.Plan,
            Result = project.Result,
            OwnerId = project.OwnerId,
            Deadline = project.Deadline,
            ApplyingDeadline = project.ApplyingDeadline,
            StateOfProject = project.StateOfProject,
            IsBusinesProject = project.IsBusinesProject,
            Budget = project.Budget,
            RequiredRoles = project.ProjectRoles.Select(pr => new RequiredRole(
                roleId: pr.RoleId,
                customRoleName: pr.CustomRoleName,
                systemRoleName: pr.Role.Name
            )).ToList()
        };
    }

    /// <summary>
    /// Создать новый проект в базе данных.
    /// </summary>
    /// <param name="projectData">Данные проекта.</param>
    /// <returns>Идентификатор созданного проекта.</returns>
    public async Task<Guid> Create(Project project)
    {
        var projectEntity = new ProjectEntity()
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            Requirements = project.Requirements,
            TeamDescription = project.TeamDescription,
            Result = project.Result,
            Plan = project.Plan,
            OwnerId = project.OwnerId,
            Deadline = project.Deadline,
            ApplyingDeadline = project.ApplyingDeadline,
            StateOfProject = project.StateOfProject,
            IsBusinesProject = project.IsBusinesProject,
            Budget = project.Budget,
            ProjectRoles = new List<ProjectRole>(),
            ProjectTags = new List<ProjectTag>()
        };

        foreach (var requiredRole in project.RequiredRoles)
        {
            var projectRole = new ProjectRole
            {
                ProjectId = project.Id,
                RoleId = requiredRole.RoleId,
                CustomRoleName = requiredRole.CustomRoleName // может быть null
            };

            projectEntity.ProjectRoles.Add(projectRole);
        }

        foreach (var tag in project.Tags)
        {
            var projectTag = new ProjectTag
            {
                ProjectId = project.Id,
                TagId = tag.Id
            };
            
            projectEntity.ProjectTags.Add(projectTag);
        }
        
        await _context.AddAsync(projectEntity);
        await _context.SaveChangesAsync();

        await _producerService.PublishAsync<Project>(project, "ProjectCreated");

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
                .SetProperty(project => project.Description, project => projectData.Description) //обновляются только 2 поля
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

    /// <summary>
    /// Возвращает общее число проектов удволетворящих фильтрам
    /// </summary>
    /// <returns></returns>
    public async Task<int> GetTotalProjectCountAsync(string searchElement, ProjectFilterDTO filters = null)
    {
        var query =  _context.Projects.AsNoTracking();
            
        query = ApplySearchFilter(query, searchElement);
        query = ApplyFilters(query, filters);
        
        return await query.CountAsync();
    }

    private  IQueryable<ProjectEntity> ApplySearchFilter(IQueryable<ProjectEntity> query, string searchElement)
    {
        return _context.Projects
            .Where(projectEntity => searchElement == string.Empty ||
                                    projectEntity.Name.ToLower().Contains(searchElement.ToLower()) ||
                                    projectEntity.ProjectRoles.Any(pr =>
                                        pr.Role.Name.ToLower().Contains(searchElement.ToLower())) ||
                                    projectEntity.ProjectRoles.Any(pr =>
                                        pr.CustomRoleName != null &&
                                        pr.CustomRoleName.ToLower().Contains(searchElement.ToLower())));
    }
    
    private IQueryable<ProjectEntity> ApplyFilters(IQueryable<ProjectEntity> query, ProjectFilterDTO filters)
    {
        if (filters == null) 
            return query;

        if (filters.RoleId != null)
        {
            query = query.Where(p => p.ProjectRoles.Any(pr => pr.RoleId == filters.RoleId));
        }

        if (filters.StateOfProject != null && filters.StateOfProject != StateOfProject.NotSelected)
        {
            query = query.Where(p => p.StateOfProject == filters.StateOfProject);
        }

        if (filters.IsCommercial.HasValue)
        {
            query = query.Where(p => p.IsBusinesProject == filters.IsCommercial.Value);
        }

        if (filters.Tags != null && filters.Tags.Count > 0)
        {
            foreach (var tagId in filters.Tags)
            {
                query = query.Where(p => p.ProjectTags.Any(pt => pt.TagId == tagId));
            }
        }

        return query;
    }
    
}