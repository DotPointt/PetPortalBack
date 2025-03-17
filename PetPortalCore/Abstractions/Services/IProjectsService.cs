using System.Diagnostics;
using PetPortalCore.Contracts;
using PetPortalCore.DTOs;
using PetPortalCore.Models;

namespace PetPortalCore.Abstractions.Services;

/// <summary>
/// Интерфейс сервиса для работы с проектами.
/// </summary>
public interface IProjectsService
{
    /// <summary>
    /// Получить все проекты.
    /// </summary>
    /// <returns>Список проектов.</returns>
    Task<List<Project>> GetAll();
    
    /// <summary>
    /// Получить проект по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор проекта.</param>
    /// <returns>Проект.</returns>
    Task<Project> GetById(Guid id);

    /// <summary>
    /// Получить проекты с пагинацией.
    /// </summary>
    /// <param name="sortOrder">Очередь сортировки.</param>
    /// <param name="sortItem">Элемент сортировки.</param>
    /// <param name="searchElement">Поиск.</param>
    /// <param name="offset">Количество проектов на странице.</param>
    /// <param name="page">Номер страницы.</param>
    /// <returns>Список отсортированных проектов.</returns>
    public Task<List<Project>> GetPaginatedFiltered(bool sortOrder, string? sortItem, string searchElement, int offset = 10, int page = 1);
        
    /// <summary>
    /// Создать новый проект.
    /// </summary>
    /// <param name="project">Данные проекта.</param>
    /// <returns>Идентификатор созданного проекта.</returns>
    Task<Guid> Create(ProjectContract project);

    /// <summary>
    /// Обновить проект.
    /// </summary>
    /// <param name="project">Данные проекта.</param>
    /// <returns>Идентификатор обновленного проекта.</returns>
    Task<Guid> Update(ProjectDto project);
        
    /// <summary>
    /// Удалить проект.
    /// </summary>
    /// <param name="id">Идентификатор проекта.</param>
    /// <returns>Идентификатор удаленного проекта.</returns>
    Task<Guid> Delete(Guid id);

    /// <summary>
    /// Проверить лимит создания проектов.
    /// </summary>
    /// <param name="ownerId">Идентификатор владельца проекта.</param>
    /// <param name="limit">Лимит проектов.</param>
    /// <returns>
    /// True - лимит не превышен,
    /// False - лимит превышен.
    /// </returns>
    Task<bool> CheckCreatingLimit(Guid ownerId, int limit);
}