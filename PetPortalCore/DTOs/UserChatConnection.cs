namespace PetPortalCore.DTOs;

/// <summary>
/// DTO подключения к чату.
/// </summary>
public class UserChatConnection
{
    /// <summary>
    /// Имя подключающегося пользователя.
    /// </summary>
    public string UserName { get; set; }
    
    /// <summary>
    /// Название комнаты чата.
    /// </summary>
    public string ChatRoom { get; set; }
}