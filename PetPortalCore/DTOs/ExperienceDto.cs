namespace PetPortalCore.DTOs;

/// <summary>
/// (Data Transfer Object) Опыт работы пользователя.
/// </summary>
public class ExperienceDto
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
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; set; }
}