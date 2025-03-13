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
    /// Идентификатор владельца проекта.
    /// </summary>
    public Guid OwnerId { get; set; }

    /// <summary>
    /// Имя владельца проекта.
    /// </summary>
    public string OwnerName { get; set; }
    
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
    public List<string> Tags { get; set; }
}