namespace PetPortalDAL.Entities.LinkingTables;

/// <summary>
/// Связующая сущность пользователей и комнаты чата. 
/// </summary>
public class ChatRoomUserEntity
{
    /// <summary>
    /// Идентификатор комнаты.
    /// </summary>
    public Guid ChatRoomId { get; set; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Пользователь.
    /// </summary>
    public UserEntity User { get; set; }
    
    /// <summary>
    /// Время последнего прочтения сообщений в комнате.
    /// </summary>
    public DateTime? LastReadAt { get; set; }
    
    /// <summary>
    /// Комната чата.
    /// </summary>
    public ChatRoomEntity ChatRoom { get; set; }
}
