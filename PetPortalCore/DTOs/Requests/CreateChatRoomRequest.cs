namespace PetPortalCore.DTOs.Requests;

/// <summary>
/// Запрос на создание комнаты чата.
/// </summary>
public class CreateChatRoomRequest
{
    /// <summary>
    /// Имя комнаты.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Идентификаторы пользователей.
    /// </summary>
    public List<Guid> UserIds { get; set; }
}