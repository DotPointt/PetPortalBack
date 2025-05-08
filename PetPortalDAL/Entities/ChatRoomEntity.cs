using PetPortalDAL.Entities.LinkingTables;

namespace PetPortalDAL.Entities;

/// <summary>
/// Сущность комнаты чата в базе данных.
/// </summary>
public class ChatRoomEntity
{
    /// <summary>
    /// Идентификатор комнаты.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Имя комнаты.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Список пользователей чата.
    /// </summary>
    public ICollection<ChatRoomUserEntity> ChatRoomUsers { get; set; }
    
    /// <summary>
    /// Список сообщений.
    /// </summary>
    public ICollection<ChatMessageEntity> Messages { get; set; }
}
