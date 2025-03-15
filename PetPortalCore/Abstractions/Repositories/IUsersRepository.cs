using PetPortalCore.DTOs;
using PetPortalCore.Models;

namespace PetPortalCore.Abstractions.Repositories;

/// <summary>
/// Интерфейс репозитория для работы с пользователями.
/// </summary>
public interface IUsersRepository
{
    /// <summary>
    /// Получить всех пользователей.
    /// </summary>
    /// <returns>Список пользователей.</returns>
    Task<List<User>> GetAll();
        
    /// <summary>
    /// Создать нового пользователя.
    /// </summary>
    /// <param name="user">Данные пользователя.</param>
    /// <returns>Идентификатор созданного пользователя.</returns>
    Task<Guid> Create(User user);

    /// <summary>
    /// Получить пользователя по электронной почте.
    /// </summary>
    /// <param name="email">Электронная почта пользователя.</param>
    /// <returns>Пользователь.</returns>
    /// <exception cref="ArgumentException">Выбрасывается, если данные пользователя невалидны.</exception>
    /// <exception cref="InvalidOperationException">Выбрасывается, если пользователь с такой почтой уже существует.</exception>
    public Task<User> GetByEmail(string email);
    
    /// <summary>
    /// Получить пользователя по идентификатору.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Пользователь.</returns>
    public Task<User> GetById(Guid userId);

    /// <summary>
    /// Обновить данные пользователя.
    /// </summary>
    /// <param name="userData">Обновленные данные пользователя.</param>
    /// <returns>Идентификатор обновленного пользователя.</returns>
    Task<Guid> Update(UserDto userData);
        
    /// <summary>
    /// Удалить пользователя.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <returns>Идентификатор удаленного пользователя.</returns>
    Task<Guid> Delete(Guid id);
}