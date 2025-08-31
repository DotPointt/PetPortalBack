using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.Contracts;
using PetPortalCore.DTOs;
using PetPortalCore.Models;
using System.Security.Cryptography;

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
    /// Репозиторий стэка пользователя.
    /// </summary>
    private readonly IStackRepository _stackRepository;
    
    /// <summary>
    /// Репозиторий опыта работы пользователя.
    /// </summary>
    private readonly IExperienceRepository _experienceRepository;
    
    /// <summary>
    /// Репозиторий образования пользователя.
    /// </summary>
    private readonly IEducationRepository _educationRepository;
    
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
    /// <param name="passwordHasher">Сервис хеширования паролей.</param>
    /// <param name="roleRepository">Репозиторий ролей.</param>
    /// <param name="stackRepository">Репозиторий стэка пользователя.</param>
    /// <param name="experienceRepository">Репозиторий опыта работы пользователя.</param>
    /// <param name="educationRepository">Репозиторий образования пользователя.</param>
    public UserService(IUsersRepository usersRepository, IProjectsRepository projectsRepository, 
        IJwtProvider jwtProvider, IPasswordHasher passwordHasher, IRoleRepository roleRepository, 
        IStackRepository stackRepository, IExperienceRepository experienceRepository, IEducationRepository educationRepository)
    {
        _projectsRepository = projectsRepository;
        _usersRepository = usersRepository;
        _jwtProvider = jwtProvider;
        _passwordHasher = passwordHasher;
        _roleRepository = roleRepository;
        _stackRepository = stackRepository;
        _experienceRepository = experienceRepository;
        _educationRepository = educationRepository;
    }
    
    /// <summary>
    /// Получить всех пользователей.
    /// </summary>
    /// <returns>Список пользователей.</returns>
    public async Task<List<UserDto>> GetAll()
    {
        var users = await _usersRepository.GetAll();

        var usersDtos = new List<UserDto>();
        
        foreach (var user in users)
        {
            var education = await _educationRepository.GetByUserId(user.Id);
            var experience = await _experienceRepository.GetByUserId(user.Id);
            var stack = await _stackRepository.GetByUserId(user.Id);
            
            usersDtos.Add(new UserDto()
            {
                Id = user.Id,
                Name = user.Name,
                City = user.City,
                Country = user.Country,
                Telegram = user.Telegram,
                Phone = user.Phone,
                Email = user.Email,
                AvatarUrl = user.AvatarUrl,
                Educations = education,
                Experiences = experience,
                Stacks = stack,
            });
        }
        
        return usersDtos;
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
    /// <exception cref="ArgumentException">Выбрасывается, если данные пользователя невалидны.</exception>
    /// <exception cref="InvalidOperationException">Выбрасывается, если пользователь с такой почтой уже существует.</exception>
    /// <returns>Идентификатор созданного пользователя.</returns>
    public async Task<Guid> Register(UserContract request)
    {
        var hashedPassword = _passwordHasher.HashPassword(request.Password);
        
        var existingUser = await _usersRepository.GetByEmail(request.Email);
        
        if (existingUser != null)
        {
            throw new InvalidOperationException("Пользователь с такой почтой уже существует.");
        }
        
        var (user, error) = PetPortalCore.Models.User.Register(
            Guid.NewGuid(),
            request.Name,
            request.Email,
            hashedPassword,
            DefaultValues.RoleId,
            string.Empty //TODO Указать путь к дефолтной аватарке.
        ); 
            
        if (!string.IsNullOrEmpty(error))
        {
            throw new ArgumentException(error);
        }
    
        return await _usersRepository.Create(user);
    }

    /// <summary>
    /// Аутентификация пользователя.
    /// </summary>
    /// <param name="email">Электронная почта пользователя.</param>
    /// <param name="password">Пароль пользователя.</param>
    /// <exception cref="UnauthorizedAccessException">Выбрасывается, если пользователь с такой почтой не найден или пароль неверен.</exception>
    /// <returns>JWT-токен.</returns>
    public async Task<string> Login(string email, string password)
    {
        var user = await _usersRepository.GetByEmail(email);

        if (user == null)
            throw new UnauthorizedAccessException("Пользователь не найден.");

        var verify = _passwordHasher.VerifyHashedPassword(user.PasswordHash, password);

        if (!verify)
            throw new UnauthorizedAccessException("Не удалось войти: неверный пароль.");

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
        if (userData.Educations is not null)
        {
            foreach (var education in userData.Educations)
            {
                if (education.IsActive)
                {
                    if (education.Id == Guid.Empty)
                    {
                        education.Id = Guid.NewGuid();
                        await _educationRepository.CreateEducation(education);
                    }
                    else
                    {
                        await _educationRepository.UpdateEducation(education); 
                    }
                }
                else
                {
                    await _educationRepository.DeleteEducation(education.Id);
                }
            }
        }

        if (userData.Experiences is not null)
        {
            foreach (var experience in userData.Experiences)
            {
                if (experience.IsActive)
                {
                    if (experience.Id == Guid.Empty)
                    {
                        experience.Id = Guid.NewGuid();
                        await _experienceRepository.CreateExperience(experience);
                    }
                    else
                    {
                        await _experienceRepository.UpdateExperience(experience);
                    }
                }
                else
                {
                    await _experienceRepository.DeleteExperience(experience.Id);
                }
                
            }
        }

        if (userData.Stacks is not null)
        {
            foreach (var stack in userData.Stacks)
            {
                if (stack.IsActive)
                {
                   if (stack.Id == Guid.Empty)
                   {
                       stack.Id = Guid.NewGuid();
                       await _stackRepository.CreateStack(stack);
                   }
                   else
                   {
                       await _stackRepository.UpdateStack(stack);
                   } 
                }
                else
                {
                    await _stackRepository.DeleteExperience(stack.Id);
                }
            }
        }
        
        return await _usersRepository.Update(userData);
    }

    /// <summary>
    /// Обновление аватара пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="avatarUrl">Путь к аватару.</param>
    /// <returns>Идентификатор обновленного пользователя.</returns>
    public async Task<Guid> UpdateAvatar(Guid userId, string avatarUrl)
    {
        return await _usersRepository.UpdateAvatar(userId, avatarUrl);
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
    public async Task<UserDto> GetUserById(Guid id)
    {
        var user = await _usersRepository.GetById(id);
        var experience = await _experienceRepository.GetByUserId(user.Id);
        var education = await _educationRepository.GetByUserId(user.Id);
        var stack = await _stackRepository.GetByUserId(user.Id);

        return new UserDto()
        {
            Id = user.Id,
            Name = user.Name,
            City = user.City,
            Country = user.Country,
            Telegram = user.Telegram,
            Phone = user.Phone,
            Email = user.Email,
            AvatarUrl = user.AvatarUrl,
            Educations = education,
            Experiences = experience,
            Stacks = stack,
        }; 
    }
    
    /// <summary>
    /// Получить пользователя по почте
    /// </summary>
    /// <returns>Объект пользователя.</returns>
    public async Task<UserDto> GetUserByEmail(string email)
    {
        var user = await _usersRepository.GetByEmail(email);
        var experience = await _experienceRepository.GetByUserId(user.Id);
        var education = await _educationRepository.GetByUserId(user.Id);
        var stack = await _stackRepository.GetByUserId(user.Id);
        
        return new UserDto()
        {
            Id = user.Id,
            Name = user.Name,
            City = user.City,
            Country = user.Country,
            Telegram = user.Telegram,
            Phone = user.Phone,
            Email = user.Email,
            AvatarUrl = user.AvatarUrl,
            Educations = education,
            Experiences = experience,
            Stacks = stack,
        }; 
    }
    
    public async Task<Guid> UpdatePasswordByIdAsync(Guid userId, string newPassword)
    {
        var user = await _usersRepository.GetById(userId);

        var userWithNewPassword = new UserDto()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            PasswordHash = _passwordHasher.HashPassword(newPassword),
            AvatarUrl = user.AvatarUrl,
            RoleId = user.RoleId,
            Country = user.Country,
            City = user.City,
            Phone = user.Phone,
            Telegram = user.Telegram,
        };
        
        return await _usersRepository.Update(userWithNewPassword);
    }
    
    
    // Метод для извлечения userId из ClaimsPrincipal
    public async Task<Guid?> GetUserIdFromJWTAsync(ClaimsPrincipal user)
    {
        Claim? userIdClaim = user.FindFirst("sub") 
                          ?? user.FindFirst(ClaimTypes.NameIdentifier);

        var userid = Guid.TryParse(userIdClaim.Value, out Guid userId);
        
        if (userIdClaim == null)
        {
            throw new UnauthorizedAccessException("Идентификатор пользователя не найден в токене.");
        }

        if (!userid)
        {
            throw new UnauthorizedAccessException("Неверный формат идентификатора пользователя.");
        }

        return userid ? userId : null;
    }
    
}