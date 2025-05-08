namespace PetPortalCore.DTOs;

/// <summary>
/// Данные по комнате чата.
/// </summary>
public class ChatRoomDto
{
    /// <summary>
    /// Идентификатор чата.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Название чата.
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// Идентификаторы участников чата.
    /// </summary>
    public List<Guid> UserIds { get; set; }
}
