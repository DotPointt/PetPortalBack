using PetPortalCore.Models;

namespace PetPortalCore.Abstractions.Repositories;

/// <summary>
/// Интерфейс репозитория для работы с участниками проектов.
/// </summary>
public interface IUserProjectRepository
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
    /// <param name="memberId">Идентификатор участника.</param>
    /// <param name="projectId">Идентификатор проекта.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Идентификатор нового участника.</returns>
    Task<Guid> AddProjectMember(Guid memberId, Guid projectId, Guid userId);

    /// <summary>
    /// Удалить участника из проекта.
    /// </summary>
    /// <param name="memberId">Идентификатор участника.</param>
    /// <returns>Идентификатор удаленного участника.</returns>
    Task<Guid> DeleteProjectMember(Guid memberId);
}