using Microsoft.AspNetCore.Identity;
using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.Contracts;
using PetPortalCore.DTOs;
using PetPortalCore.Models;

namespace PetPortalApplication.Services;

/// <summary>
/// Сервис для работы с пользователями.
/// </summary>
public class UserService : IUserService
{
    /// <summary>
    /// Репозиторий для работы с пользователями.
    /// </summary>
    private readonly IUsersRepository _usersRepository;
    
    /// <summary>
    /// Репозиторий для работы с ролями.
    /// </summary>
    private readonly IRoleRepository _roleRepository;

    /// <summary>
    /// Репозиторий для работы с проектами.
    /// </summary>
    private readonly IProjectsRepository _projectsRepository;
    
    /// <summary>
    /// Провайдер для генерации JWT-токенов.
    /// </summary>
    private readonly IJwtProvider _jwtProvider;
    
    /// <summary>
    /// Сервис для хэширования паролей.
    /// </summary>
    private readonly IPasswordHasher _passwordHasher;

    /// <summary>
    /// Конструктор сервиса пользователей.
    /// </summary>
    /// <param name="usersRepository">Репозиторий пользователей.</param>
    /// <param name="projectsRepository">Репозиторий проектов.</param>
    /// <param name="jwtProvider">Провайдер JWT-токенов.</param>
    /// <param name="passwordHasher">Сервис хэширования паролей.</param>
    /// <param name="roleRepository">Репозиторий ролей.</param>
    public UserService(IUsersRepository usersRepository, IProjectsRepository projectsRepository, 
        IJwtProvider jwtProvider, IPasswordHasher passwordHasher, IRoleRepository roleRepository)
    {
        _projectsRepository = projectsRepository;
        _usersRepository = usersRepository;
        _jwtProvider = jwtProvider;
        _passwordHasher = passwordHasher;
        _roleRepository = roleRepository;
    }
    
    /// <summary>
    /// Получить всех пользователей.
    /// </summary>
    /// <returns>Список пользователей.</returns>
    public async Task<List<User>> GetAll()
    {
        return await _usersRepository.GetAll();
    }

    /// <summary>
    /// Получить проекты, созданные пользователем.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Список проектов.</returns>
    public async Task<List<Project>> GetOwnProjects(Guid userId)
    {
        return await _projectsRepository.GetOwnProjects(userId);
    }

    /// <summary>
    /// Регистрация нового пользователя.
    /// </summary>
    /// <param name="request">Данные пользователя.</param>
    /// <returns>Идентификатор созданного пользователя.</returns>
    /// <exception cref="ArgumentException">Выбрасывается, если данные пользователя невалидны.</exception>
    /// <exception cref="InvalidOperationException">Выбрасывается, если пользователь с такой почтой уже существует.</exception>
    public async Task<Guid> Register(UserContract request)
    {
        var hashedPassword = _passwordHasher.HashPassword(request.Password);

        try
        {
            await _usersRepository.GetByEmail(request.Email);
            
            throw new InvalidOperationException("Пользователь с такой почтой уже существует.");
        }
        catch (NullReferenceException exception)
        {
            var (user, error) = PetPortalCore.Models.User.Create(
                Guid.NewGuid(),
                request.Name,
                request.Email,
                hashedPassword,
                DefaultValues.RoleId,
                string.Empty); //TODO Указать путь к дефолтной аватарке.
                
            if (!string.IsNullOrEmpty(error))
            {
                throw new ArgumentException(error);
            }
        
            return await _usersRepository.Create(user);
        }
    }
    
    /// <summary>
    /// Аутентификация пользователя.
    /// </summary>
    /// <param name="email">Электронная почта пользователя.</param>
    /// <param name="password">Пароль пользователя.</param>
    /// <returns>JWT-токен.</returns>
    /// <exception cref="Exception">Выбрасывается, если пользователь не найден или пароль неверен.</exception>
    public async Task<string> Login(string email, string password)
    {
        var user = await _usersRepository.GetByEmail(email);

        if (user == null)
            throw new Exception("Пользователь не найден.");

        var verify = _passwordHasher.VerifyHashedPassword(user.PasswordHash, password);

        if (!verify)
            throw new Exception("Не удалось войти: неверный пароль.");

        var roleName = await _roleRepository.GetRoleByUserId(user.Id);
        
        var token = _jwtProvider.GenerateToken(user.Id, user.Email, roleName);
        
        return token;
    }
    
    /// <summary>
    /// Обновление данных пользователя.
    /// </summary>
    /// <param name="userData">Обновленные данные пользователя.</param>
    /// <returns>Идентификатор обновленного пользователя.</returns>
    public async Task<Guid> Update(UserDto userData)
    {
        return await _usersRepository.Update(userData);
    }

    /// <summary>
    /// Обновление аватара пользователя.
    /// </summary>
    /// <param name="userData">Данные пользователя с новым аватаром.</param>
    /// <returns>Идентификатор обновленного пользователя.</returns>
    public async Task<Guid> UpdateAvatar(UserDto userData)
    {
        var user = await _usersRepository.GetById(userData.Id);
        
        var fullUserData = new UserDto()
        {
            Id = user.Id,            
            Name = user.Name,
            Email = user.Email,
            Password = user.PasswordHash,
            AvatarUrl = userData.AvatarUrl,
            RoleId = user.RoleId,
        };
        
        return await _usersRepository.Update(fullUserData);
    }

    /// <summary>
    /// Удаление пользователя.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <returns>Идентификатор удаленного пользователя.</returns>
    public async Task<Guid> Delete(Guid id)
    {
        return await _usersRepository.Delete(id);
    }

    /// <summary>
    /// Получить пользователя по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <returns>Объект пользователя.</returns>
    public async Task<User> GetUserById(Guid id)
    {
        return await _usersRepository.GetById(id);
    }
}