namespace PetPortalDAL.Entities;

/// <summary>
/// Опыт работы.
/// </summary>
public class ExperienceEntity
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Место работы.
    /// </summary>
    public string WorkPlace  { get; set; } 
    
    /// <summary>
    /// Позиция на работе.
    /// </summary>
    public string WorkPosition { get; set; }
    
    /// <summary>
    /// Кол-во годов работы.
    /// </summary>
    public int WorkYears { get; set; }
    
    /// <summary>
    /// Внешний ключ
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Навигационное свойство
    /// </summary>
    public UserEntity User { get; set; }
}