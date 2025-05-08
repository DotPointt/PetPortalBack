namespace PetPortalDAL.Entities;

/// <summary>
/// Сущность сообщения в базе данных.
/// </summary>
public class ChatMessageEntity
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
    /// Идентификатор комнаты чата.
    /// </summary>
    public Guid ChatRoomId { get; set; }

    /// <summary>
    /// Сообщение.
    /// </summary>
    public required string Message { get; set; }
    
    /// <summary>
    /// Дата отправки.
    /// </summary>
    public DateTime SentAt { get; set; }
    
    /// <summary>
    /// Отправитель.
    /// </summary>
    public UserEntity Sender { get; set; }
    
    /// <summary>
    /// Комната чата.
    /// </summary>
    public ChatRoomEntity ChatRoom { get; set; }
}