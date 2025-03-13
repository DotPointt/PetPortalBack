using Microsoft.EntityFrameworkCore;
using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.Models;

namespace PetPortalDAL.Repositories;

/// <summary>
/// Репозиторий для работы с ролями.
/// </summary>
public class RoleRepository : IRoleRepository
{
    /// <summary>
    /// Контекст базы данных.
    /// </summary>
    private readonly PetPortalDbContext _context;
    
    /// <summary>
    /// Репозиторий для работы с пользователями.
    /// </summary>
    private readonly IUsersRepository _usersRepository;

    /// <summary>
    /// Конструктор репозитория.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    /// <param name="usersRepository">Репозиторий для работы с пользователями.</param>
    public RoleRepository(PetPortalDbContext context, IUsersRepository usersRepository)
    {
        _context = context;
        _usersRepository = usersRepository;
    }

    /// <summary>
    /// Получить название роли пользователя по его идентификатору.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Название роли.</returns>
    /// <exception cref="KeyNotFoundException">Выбрасывается, если роль не найдена.</exception>
    public async Task<string> GetRoleByUserId(Guid userId)
    {
        var user = await _usersRepository.GetById(userId);
        
        var roleName = await _context.Roles
            .Where(role => role.Id == user.RoleId)
            .Select(role => role.Name)
            .FirstOrDefaultAsync();
        
        if (roleName is null)
            throw new KeyNotFoundException("Роль не найдена.");
        
        return roleName;
    }
}