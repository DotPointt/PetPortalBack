namespace PetPortalCore.DTOs.Requests;

/// <summary>
/// Запрос на сохранения сообщения.
/// </summary>
public class SendMessageRequest
{
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
    public string Message { get; set; }
}