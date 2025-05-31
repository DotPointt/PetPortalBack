using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.DTOs;
using PetPortalCore.Models;
using PetPortalDAL.Entities;

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

    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;
    
    /// <summary>
    /// Конструктор репозитория.
    /// </summary>
    /// <param name="context">Контекст базы данных.</param>
    public UsersRepository(PetPortalDbContext context, IMapper mapper, IPasswordHasher passwordHasher)
    {
        _context = context;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
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
     
        if (user == null && !throwNullException)
        {
            return null;
        }
        
        return User.Create(
            user.Id, 
            user.Name, 
            user.Email, 
            user.PasswordHash, 
            user.RoleId, 
            user.AvatarUrl,
            user.Country,
            user.City,
            user.Phone,
            user.Telegram
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

        return user switch
        {
            null when throwNullException => throw new Exception("User not found"),
            null => null,
            _ => User.Create(user.Id, user.Name, user.Email, user.PasswordHash, user.RoleId, user.AvatarUrl,
                    user.Country, user.City, user.Phone, user.Telegram)
                .user
        };
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
                User.Create(user.Id, user.Name, user.Email, user.PasswordHash, user.RoleId, user.AvatarUrl, user.Country, user.City, user.Phone, user.Telegram).user)
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
            Country = user.Country,
            City = user.City,
            Phone = user.Phone,
            Telegram = user.Telegram,
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
        var existingUserEntity = await _context.Users
            .FirstOrDefaultAsync(p => p.Id == userData.Id);
        
        var mappedUser = userData.Adapt<UserEntity>();
        
        _context.Entry((UserEntity)existingUserEntity).CurrentValues.SetValues(mappedUser);

        await _context.SaveChangesAsync();
        return userData.Id;
    }

    /// <summary>
    /// Обновить только аватар.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="avatarUrl">путь к файлу аватара.</param>
    /// <returns>Идентификатор пользователя.</returns>
    public async Task<Guid> UpdateAvatar(Guid userId, string avatarUrl)
    {
        await _context.Users
            .Where(user => user.Id == userId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(user => user.AvatarUrl, avatarUrl)
            );

        return userId;
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