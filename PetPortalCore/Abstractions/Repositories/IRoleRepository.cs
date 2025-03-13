namespace PetPortalCore.Abstractions.Repositories;

/// <summary>
/// Интерфейс репозитория для работы с ролями пользователей.
/// </summary>
public interface IRoleRepository
{
    /// <summary>
    /// Получить название роли пользователя по его идентификатору.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Название роли.</returns>
    Task<string> GetRoleByUserId(Guid userId);
}