using Microsoft.EntityFrameworkCore;
using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.Models;
using PetPortalDAL.Entities;
using PetPortalDAL.Entities.LinkingTables;

namespace PetPortalDAL.Repositories;

/// <summary>
/// Репозиторий для работы с участниками проектов.
/// </summary>
public class UserProjectRepository : IUserProjectRepository
{
    /// <summary>
    /// Контекст базы данных.
    /// </summary>
    private readonly PetPortalDbContext _context;
    
    /// <summary>
    /// Конструктор репозитория.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    public UserProjectRepository(PetPortalDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Получить список участников проекта по идентификатору проекта.
    /// </summary>
    /// <param name="projectId">Идентификатор проекта.</param>
    /// <returns>Список участников проекта.</returns>
    public async Task<List<User>> GetProjectMembers(Guid projectId)
    {
        var projectMemberIds = await _context.UserProjects
            .AsNoTracking()
            .Where(e => e.ProjectId == projectId)
            .Select(e => e.UserId)
            .ToListAsync();
        
        var users = await _context.Users
            .AsNoTracking()
            .Where(e => projectMemberIds.Contains(e.Id))
            .Select(user =>
                User.Create(user.Id, user.Name, user.Email, user.PasswordHash, user.RoleId, user.AvatarUrl).user)
            .ToListAsync();

        return users;
    }

    /// <summary>
    /// Добавить нового участника в проект.
    /// </summary>
    /// <param name="memberId">Идентификатор участника.</param>
    /// <param name="projectId">Идентификатор проекта.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Идентификатор нового участника.</returns>
    public async Task<Guid> AddProjectMember(Guid memberId, Guid projectId, Guid userId)
    {
        var newProjectMember = new UserProject()
        {
            Id = memberId,
            ProjectId = projectId,
            UserId = userId
        };
        
        await _context.UserProjects.AddAsync(newProjectMember);
        await _context.SaveChangesAsync();
        
        return newProjectMember.Id;
    }

    /// <summary>
    /// Удалить участника из проекта.
    /// </summary>
    /// <param name="memberId">Идентификатор участника.</param>
    /// <returns>Идентификатор удаленного участника.</returns>
    public async Task<Guid> DeleteProjectMember(Guid memberId)
    {
        await _context.UserProjects
            .Where(m => m.Id == memberId)
            .ExecuteDeleteAsync();
        
        await _context.SaveChangesAsync();
        return memberId;
    }
}