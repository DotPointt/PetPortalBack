using PetPortalCore.Models;

namespace PetPortalCore.DTOs;

/// <summary>
/// DTO (Data Transfer Object) для проекта.
/// </summary>
public class ProjectDto
{
    /// <summary>
    /// Идентификатор проекта.
    /// </summary>
    public Guid Id { get; set; }
        
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
    public string Requirements { get; set; } = string.Empty ;
    
    /// <summary>
    /// Description of current and future team members
    /// </summary>
    public string TeamDescription { get; set; } = string.Empty;

    /// <summary>
    /// Plan of project
    /// </summary>
    public string Plan { get; set; } = string.Empty; 
        
    /// <summary>
    /// Description of awaited result of project
    /// </summary>
    public string Result { get; set; } = string.Empty;
        
    /// <summary>
    /// Идентификатор владельца проекта.
    /// </summary>
    public Guid OwnerId { get; set; }
    
    /// <summary>
    /// Имя владельца.
    /// </summary>
    public string OwnerName { get; set; }
    
    /// <summary>
    /// Срок завершения проекта. Если null, проект считается бессрочным.
    /// </summary>
    public DateTime? Deadline { get; set; }

    /// <summary>
    /// Срок подачи заявок на участие в проекте. Если null, срок считается бессрочным.
    /// </summary>
    public DateTime? ApplyingDeadline { get; set; } = null;

    /// <summary>
    /// Бюджет проекта в рублях.
    /// </summary>
    public uint Budget { get; set; } = 0;

    /// <summary>
    /// Текущее состояние проекта (открыт/закрыт для участия).
    /// </summary>
    public StateOfProject StateOfProject { get; set; } = StateOfProject.Closed;
    
    /// <summary>
    /// Указывает, является ли проект коммерческим.
    /// </summary>
    public bool IsBusinessProject { get; set; } = false;
    
    /// <summary>
    /// Аватар проекта в формате Base64.
    /// </summary>
    public string AvatarImageBase64 { get; set; }
    
    /// <summary>
    /// Tags related to Project. List of strings.
    /// </summary>
    public List<string> Tags { get; set; }
}