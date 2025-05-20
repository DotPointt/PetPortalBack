namespace PetPortalCore.DTOs;

/// <summary>
/// (Data Transfer Object) Стэк пользователя.
/// </summary>
public class StackDto
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Язык программирования.
    /// </summary>
    public string ProgrammingLanguage { get; set; }
    
    /// <summary>
    /// Уровень программирования.
    /// </summary>
    public int ProgrammingLevel { get; set; }
    
    /// <summary>
    /// Опыт программирования в годах.
    /// </summary>
    public int ProgrammingYears { get; set; }
    
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; set; }
}