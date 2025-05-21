using PetPortalDAL.Entities.LinkingTables;

namespace PetPortalDAL.Entities;

/// <summary>
/// Сущность пользователя в базе данных.
/// </summary>
public class UserEntity
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Страна проживания.
    /// </summary>
    public string? Country { get; set; }
    
    /// <summary>
    /// Город проживания.
    /// </summary>
    public string? City { get; set; }
    
    /// <summary>
    /// Номер телефона.
    /// </summary>
    public string? Phone { get; set; }
    
    /// <summary>
    /// Телеграм аккаунт.
    /// </summary>
    public string? Telegram { get; set; }
    
    /// <summary>
    /// Электронная почта пользователя.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Хэш пароля пользователя.
    /// </summary>
    public string PasswordHash { get; set; }
    
    /// <summary>
    /// Идентификатор роли пользователя.
    /// </summary>
    public Guid RoleId { get; set; }
    
    /// <summary>
    /// Путь к файлу аватара пользователя.
    /// </summary>
    public string? AvatarUrl { get; set; }
    
    /// <summary>
    /// Роль пользователя.
    /// </summary>
    public RoleEntity RoleEntity { get; set; }
    
    /// <summary>
    /// Список проектов, связанных с пользователем.
    /// </summary>
    public ICollection<UserProject> UserProjects { get; set; }
    
    /// <summary>
    /// Образования.
    /// </summary>
    public ICollection<EducationEntity> Educations { get; set; } = new List<EducationEntity>();
    
    /// <summary>
    /// Опыт работы.
    /// </summary>
    public ICollection<ExperienceEntity> Experiences { get; set; } = new List<ExperienceEntity>();
    
    /// <summary>
    /// Стэки
    /// </summary>
    public ICollection<StackEntity> Stacks { get; set; } = new List<StackEntity>();
}