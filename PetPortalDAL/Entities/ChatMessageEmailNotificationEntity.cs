namespace PetPortalDAL.Entities;

/// <summary>
/// Лог отправленных email-уведомлений о непрочитанных сообщениях.
/// </summary>
public class ChatMessageEmailNotificationEntity
{
    public Guid Id { get; set; }
    public Guid MessageId { get; set; }
    public Guid UserId { get; set; }
    public DateTime SentAt { get; set; }

    public ChatMessageEntity Message { get; set; } = null!;
    public UserEntity User { get; set; } = null!;
}
