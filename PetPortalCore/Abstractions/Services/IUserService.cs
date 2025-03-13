using PetPortalCore.Contracts;
using PetPortalCore.DTOs;
using PetPortalCore.Models;

namespace PetPortalCore.Abstractions.Services;

/// <summary>
/// Интерфейс сервиса для работы с пользователями.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Получить всех пользователей.
    /// </summary>
    /// <returns>Список пользователей.</returns>
    Task<List<User>> GetAll();

    /// <summary>
    /// Получить проекты, созданные пользователем.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Список проектов.</returns>
    Task<List<Project>> GetOwnProjects(Guid userId);
    
    /// <summary>
    /// Регистрация нового пользователя.
    /// </summary>
    /// <param name="user">Данные пользователя.</param>
    /// <returns>Идентификатор созданного пользователя.</returns>
    Task<Guid> Register(UserContract user);

    /// <summary>
    /// Аутентификация пользователя.
    /// </summary>
    /// <param name="email">Электронная почта пользователя.</param>
    /// <param name="password">Пароль пользователя.</param>
    /// <returns>JWT-токен.</returns>
    Task<string> Login(string email, string password);

    /// <summary>
    /// Обновление данных пользователя.
    /// </summary>
    /// <param name="userData">Обновленные данные пользователя.</param>
    /// <returns>Идентификатор обновленного пользователя.</returns>
    Task<Guid> Update(UserDto userData);

    /// <summary>
    /// Обновление аватара пользователя.
    /// </summary>
    /// <param name="userData">Обновленные данные пользователя.</param>
    /// <returns>Идентификатор обновленного пользователя.</returns>
    Task<Guid> UpdateAvatar(UserDto userData);

    /// <summary>
    /// Удаление пользователя.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <returns>Идентификатор удаленного пользователя.</returns>
    Task<Guid> Delete(Guid id);

    /// <summary>
    /// Получить пользователя по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <returns>Объект пользователя.</returns>
    public Task<User> GetUserById(Guid id);
}