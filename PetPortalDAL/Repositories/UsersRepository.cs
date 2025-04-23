using Microsoft.EntityFrameworkCore;
using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.DTOs;
using PetPortalCore.Models;
using PetPortalDAL.Entities;
using Mapster;

namespace PetPortalDAL.Repositories;

/// <summary>
/// Репозиторий пользователей.
/// </summary>
public class UsersRepository : IUsersRepository
{
    /// <summary>
    /// Контекст базы данных.
    /// </summary>
    private readonly PetPortalDbContext _context;
        
    /// <summary>
    /// Конструктор репозитория.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    public UsersRepository(PetPortalDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Получить пользователя по email.
    /// </summary>
    /// <param name="email">Email пользователя.</param>
    /// <returns>Пользователь если имеется, в ином случае null.</returns>
    public async Task<User> GetByEmail(string email, bool throwNullException = false)
    { 
        var user = await _context.Users
            .AsNoTracking()
            .Where(user => user.Email == email)
            .FirstOrDefaultAsync();
     
        if (user == null && throwNullException)
        {
            return null;
        }
        
        return User.Create(
            user.Id, 
            user.Name, 
            user.Email, 
            user.PasswordHash, 
            user.RoleId, 
            user.AvatarUrl
        ).user;
    }

    /// <summary>
    /// Получить пользователя по идентификатору.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    public async Task<User> GetById(Guid userId, bool throwNullException = false)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Where(user => user.Id == userId)
            .FirstOrDefaultAsync();
        
        if (user == null && throwNullException)
            throw new Exception("User not found");
        else if (user == null)
            return null;
        
        return User.Create(user.Id, user.Name, user.Email, user.PasswordHash, user.RoleId, user.AvatarUrl).user;
    }
    
    
    /// <summary>
    /// Получить всех пользователей из базы данных.
    /// </summary>
    /// <returns>Список пользователей.</returns>
    public async Task<List<User>> GetAll()
    {
        var userEntities = await _context.Users
            .AsNoTracking()
            .ToListAsync();

        var users = userEntities
            .Select(user =>
                User.Create(user.Id, user.Name, user.Email, user.PasswordHash, user.RoleId, user.AvatarUrl).user)
            .ToList();

        return users;
    }

    /// <summary>
    /// Создать нового пользователя в базе данных.
    /// </summary>
    /// <param name="user">Данные пользователя.</param>
    /// <returns>Идентификатор созданного пользователя.</returns>
    public async Task<Guid> Create(User user)
    {
        var userEntity = new UserEntity()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            PasswordHash = user.PasswordHash,
            RoleId = user.RoleId,
            AvatarUrl = user.AvatarUrl,
        };

        await _context.AddAsync(userEntity);
        await _context.SaveChangesAsync();

        return userEntity.Id;
    }

    /// <summary>
    /// Обновить данные пользователя в базе данных.
    /// </summary>
    /// <param name="userData">Обновленные данные пользователя.</param>
    /// <returns>Идентификатор обновленного пользователя.</returns>
    public async Task<Guid> Update(UserDto userData)
    {
        await _context.Users
            .Where(user => user.Id == userData.Id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.Name, userData.Name)
                .SetProperty(u => u.AvatarUrl, userData.AvatarUrl)
            );

        return userData.Id;
    }

    /// <summary>
    /// Удалить пользователя из базы данных.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <returns>Идентификатор удаленного пользователя.</returns>
    public async Task<Guid> Delete(Guid id)
    {
        await _context.Users
            .Where(user => user.Id == id)
            .ExecuteDeleteAsync();

        return id;
    }
}