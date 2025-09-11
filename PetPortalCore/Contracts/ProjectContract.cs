using PetPortalCore.Models;

namespace PetPortalCore.Contracts;

/// <summary>
/// Контракт для создания проекта.
/// </summary>
public class ProjectContract
{
    /// <summary>
    /// Название проекта.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Описание проекта.
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// Description of requirements needed to be satisfied to complete the projects
    /// </summary>
    public string? Requirements { get; set; } = string.Empty ;
    
    /// <summary>
    /// Description of current and future team members
    /// </summary>
    public string? TeamDescription { get; set; } = string.Empty;

    /// <summary>
    /// Plan of project
    /// </summary>
    public string? Plan { get; set; } = string.Empty; 
        
    /// <summary>
    /// Description of awaited result of project
    /// </summary>
    public string? Result { get; set; } = string.Empty;

    // /// <summary>
    // /// Идентификатор владельца проекта.
    // /// </summary>
    // public Guid OwnerId { get; set; }

    // /// <summary>
    // /// Имя владельца проекта.
    // /// </summary>
    // public string OwnerName { get; set; }
    
    /// <summary>
    /// Срок завершения проекта. Если null, проект считается бессрочным.
    /// </summary>
    public DateTime? Deadline  { get; set; }

    /// <summary>
    /// Срок подачи заявок на участие в проекте. Если null, срок считается бессрочным.
    /// </summary>
    public DateTime? ApplyingDeadline { get; set; } = null;

    /// <summary>
    /// Текущее состояние проекта (открыт/закрыт для участия).
    /// </summary>
    public StateOfProject StateOfProject = StateOfProject.Closed;

    /// <summary>
    /// Бюджет проекта в рублях.
    /// </summary>
    public uint Budget;

    /// <summary>
    /// Указывает, является ли проект коммерческим.
    /// </summary>
    public bool IsBusinessProject = false;

    /// <summary>
    /// Список тегов, связанных с проектом.
    /// </summary>
    public List<Tag> Tags { get; set; } = new();
    
    /// <summary>
    /// Список требуемых ролей для проекта.
    /// </summary>
    public List<RequiredRole> RequiredRoles { get; set; } = new();
    
    /// <summary>
    /// If project is to be done for money
    /// </summary>
    public bool IsBusinesProject { get; set; } = false;

    // /// <summary>
    // /// Аватарка в кодировке base64.
    // /// </summary>
    // public string AvatarImageBase64 { get; set; }
}