namespace PetPortalCore.Abstractions;

/// <summary>
/// Методы клиента для работы с чатом.
/// </summary>
public interface IChatClient
{
    /// <summary>
    ///  Получение сообщения.
    /// </summary>
    /// <param name="userName">Имя пользователя.</param>
    /// <param name="message">Сообщение от пользователя.</param>
    public Task ReceiveMessage(string userName, string message);
}