using PetPortalCore.DTOs;

namespace PetPortalCore.Abstractions.Repositories;

public interface IStackRepository
{
    /// <summary>
    /// Получить стэк пользователя по идентификатору.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Список стэка пользователя.</returns>
    Task<List<StackDto>> GetByUserId(Guid userId);

    /// <summary>
    /// Добавить стэк.
    /// </summary>
    /// <param name="stackDto">Список стэков пользователя.</param>
    Task CreateStack(StackDto stackDto);

    /// <summary>
    /// Обновление стэка пользователя.
    /// </summary>
    /// <param name="stackDto">Стэк.</param>
    /// <returns>Идентификатор обновленного стэка.</returns>
    Task<Guid> UpdateStack(StackDto stackDto);

    /// <summary>
    /// Удаление стэка пользователя.
    /// </summary>
    /// <param name="stackId">Идентификатор стэка пользователя.</param>
    Task DeleteExperience(Guid stackId);
}