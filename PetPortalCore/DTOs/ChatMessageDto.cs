namespace PetPortalCore.DTOs;

/// <summary>
/// DTO (Data Transfer Object) для сообщения.
/// </summary>
public class ChatMessageDto
{
    /// <summary>
    /// Идентификатор сообщения.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Идентификатор отправителя.
    /// </summary>
    public Guid SenderId { get; set; }
    
    /// <summary>
    /// Идентификатор чата.
    /// </summary>
    public Guid ChatRoomId { get; set; }
    
    /// <summary>
    /// Сообщение.
    /// </summary>
    public string Message { get; set; }
    
    /// <summary>
    /// Дата отправки.
    /// </summary>
    public DateTime SentAt { get; set; }
    
    /// <summary>
    /// Имя отправителя.
    /// </summary>
    public string? SenderName { get; set; }
}