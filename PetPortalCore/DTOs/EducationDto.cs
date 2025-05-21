namespace PetPortalCore.DTOs;

/// <summary>
/// DTO (Data Transfer Object) Образование пользователя.
/// </summary>
public class EducationDto
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; set; } = Guid.Empty;
    
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
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Флаг для удаления не использующихся образований.
    /// </summary>
    public bool IsActive { get; set; } = true;
}