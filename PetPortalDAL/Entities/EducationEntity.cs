namespace PetPortalDAL.Entities;

/// <summary>
/// Образование.
/// </summary>
public class EducationEntity
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Университет.
    /// </summary>
    public string University { get; set; }
        
    /// <summary>
    /// Специальность.
    /// </summary>
    public string Speciality { get; set; }
    
    /// <summary>
    /// Год выпуска.
    /// </summary>
    public int ReleaseYear { get; set; }
    
    /// <summary>
    /// Внешний ключ.
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Навигационное свойство.
    /// </summary>
    public UserEntity User { get; set; }
}