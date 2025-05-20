using PetPortalCore.DTOs;

namespace PetPortalCore.Abstractions.Repositories;

public interface IEducationRepository
{
    /// <summary>
    /// Получить образования пользователя по идентификатору.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Список образований.</returns>
    Task<List<EducationDto>> GetByUserId(Guid userId);

    /// <summary>
    /// Создать новые образования.
    /// </summary>
    /// <param name="educations">Образования.</param>
    /// <param name="userId">Идентифкатор пользователя.</param>
    Task CreateEducations(List<EducationDto> educations, Guid userId);

    /// <summary>
    /// Обновление образования пользователя.
    /// </summary>
    /// <param name="education">Образование.</param>
    /// <returns>Идентификатор обновленного образования.</returns>
    Task<Guid> UpdateEducation(EducationDto education);

    /// <summary>
    /// Удаление образования пользователя.
    /// </summary>
    /// <param name="educationId">Идентификатор образования.</param>
    Task DeleteEducation(Guid educationId);
}