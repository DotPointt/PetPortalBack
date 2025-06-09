using PetPortalCore.DTOs;
using PetPortalCore.Models;

namespace PetPortalCore.Abstractions.Repositories;

/// <summary>
/// Интерфейс репозитория для работы с проектами.
/// </summary>
public interface IProjectsRepository
{
    /// <summary>
    /// Получить проекты с пагинацией.
    /// </summary>
    /// <param name="sortOrder">Очередь сортировки.</param>
    /// <param name="sortItem">Элемент сортировки.</param>
    /// <param name="searchElement">Поиск.</param>
    /// <param name="offset">Количество проектов на странице.</param>
    /// <param name="page">Номер страницы.</param>
    /// <returns>Список отсортированных проектов.</returns>
    Task<List<Project>> Get(bool sortOrder, string? sortItem, string searchElement, int offset = 10, int page = 1, ProjectFilterDTO filters = null);

    /// <summary>
    /// Получить проекты, созданные определенным пользователем.
    /// </summary>
    /// <param name="ownerId">Идентификатор пользователя.</param>
    /// <returns>Список проектов.</returns>
    Task<List<Project>> GetOwnProjects(Guid ownerId);

    /// <summary>
    /// Получить все проекты.
    /// </summary>
    /// <returns>Список проектов.</returns>
    public Task<List<Project>> GetAll();

    /// <summary>
    /// Получить проект по идентификатору.
    /// </summary>
    /// <param name="projectId">Идентификатор проекта.</param>
    /// <returns>Проект.</returns>
    public Task<Project> GetById(Guid projectId);
    
    /// <summary>
    /// Создать новый проект в базе данных.
    /// </summary>
    /// <param name="project">Данные проекта.</param>
    /// <returns>Идентификатор созданного проекта.</returns>
    Task<Guid> Create(Project project);

    /// <summary>
    /// Обновить проект в базе данных.
    /// </summary>
    /// <param name="project">Данные проекта.</param>
    /// <returns>Идентификатор обновленного проекта.</returns>
    Task<Guid> Update(ProjectDto project);
        
    /// <summary>
    /// Удалить проект из базы данных.
    /// </summary>
    /// <param name="id">Идентификатор проекта.</param>
    /// <returns>Идентификатор удаленного проекта.</returns>
    Task<Guid> Delete(Guid id);

    /// <summary>
    /// Получить количество проектов, созданных определенным пользователем.
    /// </summary>
    /// <param name="ownerId">Идентификатор пользователя.</param>
    /// <returns>Количество проектов.</returns>
    Task<int> GetProjectCountByOwnerIdAsync(Guid ownerId);

    /// <summary>
    /// Возвращает общее число проектов удволетворящих фильтрам
    /// </summary>
    /// <returns></returns>
    Task<int> GetTotalProjectCountAsync(string searchElement);
}