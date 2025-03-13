using PetPortalCore.Models;

namespace PetPortalCore.Abstractions.Services;

/// <summary>
/// Интерфейс сервиса для работы с участниками проектов.
/// </summary>
public interface IUserProjectService
{
    /// <summary>
    /// Получить список участников проекта по идентификатору проекта.
    /// </summary>
    /// <param name="projectId">Идентификатор проекта.</param>
    /// <returns>Список участников проекта.</returns>
    Task<List<User>> GetProjectMembers(Guid projectId);

    /// <summary>
    /// Добавить нового участника в проект.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="projectId">Идентификатор проекта.</param>
    /// <returns>Идентификатор нового участника.</returns>
    Task<Guid> AddProjectMember(Guid userId, Guid projectId);

    /// <summary>
    /// Удалить участника из проекта.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="projectId">Идентификатор проекта.</param>
    /// <returns>Идентификатор удаленного участника.</returns>
    Task<Guid> DeleteProjectMember(Guid userId, Guid projectId);
}