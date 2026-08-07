using PetPortalCore.DTOs;

namespace PetPortalCore.Abstractions;

/// <summary>
/// Методы клиента для работы с чатом.
/// </summary>
public interface IChatClient
{
    /// <summary>
    /// Получение нового сообщения в реальном времени.
    /// </summary>
    Task ReceiveMessage(ChatMessageDto message);
}
