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