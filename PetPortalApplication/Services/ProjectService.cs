using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.Contracts;
using PetPortalCore.DTOs;
using PetPortalCore.Models;

namespace PetPortalApplication.Services;

/// <summary>
/// Сервис для работы с проектами.
/// </summary>
public class ProjectService : IProjectsService
{
    /// <summary>
    /// Репозиторий для работы с проектами.
    /// </summary>
    private readonly IProjectsRepository _projectsRepository; 
    
    /// <summary>
    /// Конструктор сервиса проектов.
    /// </summary>
    /// <param name="projectsRepository">Репозиторий проектов.</param>
    public ProjectService(IProjectsRepository projectsRepository)
    {
        _projectsRepository = projectsRepository;
    }
    
    /// <summary>
    /// Получить все проекты.
    /// </summary>
    /// <returns>Список проектов.</returns>
    public async Task<List<Project>> GetAll()
    {
        return await _projectsRepository.GetAll();
    }

    /// <summary>
    /// Получить проекты с пагинацией.
    /// </summary>
    /// <param name="sortOrder">Очередь сортировки.</param>
    /// <param name="sortItem">Элемент сортировки.</param>
    /// <param name="searchElement">Поиск.</param>
    /// <param name="offset">Количество элементов на одной странице.</param>
    /// <param name="page">Номер страницы.</param>
    /// <returns>Отсортированный список проектов.</returns>
    public async Task<List<Project>> GetPaginatedFiltered(bool sortOrder, string? sortItem, string searchElement, int offset = 10, int page = 1, ProjectFilterDTO filters = null)
    {
        return await _projectsRepository.Get(sortOrder, sortItem, searchElement, offset, page, filters);
    }

    /// <summary>
    /// Получить проект по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор проекта.</param>
    /// <returns>Проект.</returns>
    public async Task<Project> GetById(Guid id)
    {
        return await _projectsRepository.GetById(id);
    }

    /// <summary>
    /// Создание проекта.
    /// </summary>
    /// <param name="request">Данные для создания проекта.</param>
    /// <returns>Идентификатор созданного проекта.</returns>
    /// <exception cref="ArgumentException">Выбрасывается, если данные проекта невалидны.</exception>
    public async Task<Guid> Create(ProjectContract request)
    {
        var (project, error) = Project.Create(
            id: Guid.NewGuid(),
            name: request.Name,
            description: request.Description,
            requirements: request.Requirements,
            teamDescription: request.TeamDescription,
            result: request.Result,
            plan: request.Plan,
            ownerId: request.OwnerId,
            ApplyingDeadline: request.ApplyingDeadline,
            StateOfProject: request.StateOfProject,
            Deadline: request.Deadline,
             IsBusinesProject: request.IsBusinesProject,   // <-- добавлено
            Budget: request.Budget );
        
        if (!string.IsNullOrEmpty(error))
        {
            throw new ArgumentException(error);
        }
        
        return await _projectsRepository.Create(project);
    }

    /// <summary>
    /// Обновление проекта.
    /// </summary>
    /// <param name="project">Данные проекта для обновления.</param>
    /// <returns>Идентификатор обновленного проекта.</returns>
    public async Task<Guid> Update(ProjectDto project)
    {
        return await _projectsRepository.Update(project);
    }

    /// <summary>
    /// Удаление проекта.
    /// </summary>
    /// <param name="id">Идентификатор проекта.</param>
    /// <returns>Идентификатор удаленного проекта.</returns>
    public async Task<Guid> Delete(Guid id)
    {
        return await _projectsRepository.Delete(id);
    }

    /// <summary>
    /// Проверка лимита создания проектов.
    /// </summary>
    /// <param name="ownerId">Идентификатор владельца проекта.</param>
    /// <param name="limit">Лимит проектов.</param>
    /// <returns>
    /// True - лимит не превышен,
    /// False - лимит превышен.
    /// </returns>
    public async Task<bool> CheckCreatingLimit(Guid ownerId, int limit)
    {
        var cnt = await _projectsRepository.GetProjectCountByOwnerIdAsync(ownerId);

        return cnt <= limit;
    }


    /// <summary>
    /// Возвращает общее число проектов удволетворящих фильтрам
    /// </summary>
    /// <returns></returns>
    public async Task<int> GetTotalProjectCountAsync(string searchElement)
    {
        return await _projectsRepository.GetTotalProjectCountAsync( searchElement);
    }
}