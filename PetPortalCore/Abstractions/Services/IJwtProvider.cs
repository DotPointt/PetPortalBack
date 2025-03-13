using PetPortalCore.Models;

namespace PetPortalCore.Abstractions.Services;

/// <summary>
/// Интерфейс для генерации JWT-токенов.
/// </summary>
public interface IJwtProvider
{
    /// <summary>
    /// Сгенерировать JWT-токен.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="email">Электронная почта пользователя.</param>
    /// <param name="roleName">Название роли пользователя.</param>
    /// <returns>JWT-токен.</returns>
    public string GenerateToken(Guid userId, string email, string roleName);
}