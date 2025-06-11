using PetPortalCore.Models;
using PetPortalDAL.Entities.LinkingTables;

namespace PetPortalDAL.Entities;

/// <summary>
/// Сущность проекта в базе данных.
/// </summary>
public class ProjectEntity : BaseAuditableEntity
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
    public string Requirements { get; set; }
    
    /// <summary>
    /// Description of current and future team members
    /// </summary>
    public string TeamDescription { get; set; }

    /// <summary>
    /// Plan of project
    /// </summary>
    public string Plan { get; set; }
        
    /// <summary>
    /// Description of awaited result of project
    /// </summary>
    public string Result { get; set; }

    /// <summary>
    /// Идентификатор владельца проекта.
    /// </summary>
    public Guid OwnerId { get; set; }
        
    /// <summary>
    /// Владелец проекта.
    /// </summary>
    public UserEntity Owner { get; set; }
    
    /// <summary>
    /// Срок завершения проекта. Если null, проект считается бессрочным.
    /// </summary>
    public DateTime? Deadline { get; set; } = null;

    /// <summary>
    /// Срок подачи заявок на участие в проекте. Если null, срок считается бессрочным.
    /// </summary>
    public DateTime? ApplyingDeadline { get; set; } = null;

    /// <summary>
    /// Текущее состояние проекта (открыт/закрыт для участия).
    /// </summary>
    public StateOfProject StateOfProject { get; set; } = StateOfProject.Closed;
    
    /// <summary>
    /// If project is to be done for money
    /// </summary>
    public bool IsBusinesProject { get; set; } = false;

    /// <summary>
    /// Бюджет проекта в рублях.
    /// </summary>
    public uint Budget { get; set; }

    /// <summary>
    /// Список тегов, связанных с проектом.
    /// </summary>
    public ICollection<ProjectTag> ProjectTags { get; set; } = new List<ProjectTag>();
}