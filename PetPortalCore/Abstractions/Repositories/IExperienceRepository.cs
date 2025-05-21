using PetPortalCore.DTOs;

namespace PetPortalCore.Abstractions.Repositories;

/// <summary>
/// Репозиторий опыта работы пользователя.
/// </summary>
public interface IExperienceRepository
{
    /// <summary>
    /// Получить опыт работы пользователя по идентификатору.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Список опыта работы.</returns>
    Task<List<ExperienceDto>> GetByUserId(Guid userId);

    /// <summary>
    /// Добавить опыт работы.
    /// </summary>
    /// <param name="experienceDto">Список опыта работы.</param>
    Task CreateExperience(ExperienceDto experienceDto);

    /// <summary>
    /// Обновление опыта работы пользователя.
    /// </summary>
    /// <param name="experience">Опыт работы.</param>
    /// <returns>Идентификатор обновленного опыта работы.</returns>
    Task<Guid> UpdateExperience(ExperienceDto experience);

    /// <summary>
    /// Удаление опыта работы пользователя.
    /// </summary>
    /// <param name="experienceId">Идентификатор опыта работы.</param>
    Task DeleteExperience(Guid experienceId);
}