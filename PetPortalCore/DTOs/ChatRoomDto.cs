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
    public List<Guid> UserIds { get; set; } = new();

    /// <summary>
    /// Имена участников: userId → display name.
    /// </summary>
    public Dictionary<Guid, string> ParticipantNames { get; set; } = new();

    /// <summary>
    /// Текст последнего сообщения.
    /// </summary>
    public string? LastMessage { get; set; }

    /// <summary>
    /// Время последнего сообщения.
    /// </summary>
    public DateTime? LastMessageTime { get; set; }
}
