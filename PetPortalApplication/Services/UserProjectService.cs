using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.Models;

namespace PetPortalApplication.Services;

/// <summary>
/// Сервис для управления участниками проектов.
/// </summary>
public class UserProjectService : IUserProjectService
{
    /// <summary>
    /// Репозиторий для работы с участниками проектов.
    /// </summary>
    private readonly IUserProjectRepository _userProjectRepository; 
    
    /// <summary>
    /// Конструктор сервиса участников проектов.
    /// </summary>
    /// <param name="userProjectRepository">Репозиторий участников проектов.</param>
    public UserProjectService(IUserProjectRepository userProjectRepository)
    {
        _userProjectRepository = userProjectRepository;
    }

    /// <summary>
    /// Получить список участников проекта по идентификатору проекта.
    /// </summary>
    /// <param name="projectId">Идентификатор проекта.</param>
    /// <returns>Список участников проекта.</returns>
    public async Task<List<User>> GetProjectMembers(Guid projectId)
    {
        return await _userProjectRepository.GetProjectMembers(projectId);
    }

    /// <summary>
    /// Добавить нового участника в проект.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="projectId">Идентификатор проекта.</param>
    /// <returns>Идентификатор нового участника.</returns>
    public async Task<Guid> AddProjectMember(Guid userId, Guid projectId)
    {
        var guid = Guid.NewGuid();
        return await _userProjectRepository.AddProjectMember(userId, projectId, guid);
    }

    /// <summary>
    /// Удалить участника из проекта.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="projectId">Идентификатор проекта.</param>
    /// <returns>Идентификатор удаленного участника.</returns>
    public async Task<Guid> DeleteProjectMember(Guid userId, Guid projectId)
    {
        var members = await _userProjectRepository.GetProjectMembers(projectId);
        var memberId = members
            .Where(m => m.Id == userId)
            .Select(m => m.Id)
            .FirstOrDefault();
        
        return await _userProjectRepository.DeleteProjectMember(memberId);
    }
}