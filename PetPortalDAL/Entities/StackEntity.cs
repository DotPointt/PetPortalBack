namespace PetPortalDAL.Entities;

/// <summary>
/// Стэк.
/// </summary>
public class StackEntity
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
    /// Внешний ключ
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Навигационное свойство
    /// </summary>
    public UserEntity User { get; set; }
}