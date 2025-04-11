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
    /// Название чата.
    /// </summary>
    public string ChatRoom { get; set; }
    
    /// <summary>
    /// Имя отправителя.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Сообщение.
    /// </summary>
    public string Message { get; set; }
    
    /// <summary>
    /// Дата отправки.
    /// </summary>
    public DateTime SentAt { get; set; }
}