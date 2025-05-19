using PetPortalCore.DTOs;

namespace PetPortalCore.Abstractions.Services;

/// <summary>
/// Интерфейс сервиса для работы с сообщениями.
/// </summary>
public interface IChatMessageService
{
    /// <summary>
    /// Записать сообщение в бд.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    /// <param name="senderId">Идентификатор отправителя.</param>
    /// <param name="chatId">Идентификатор чата.</param>
    /// <returns>Идентификатор сообщения.</returns>
    Task<Guid> AddAsync(string message, Guid senderId, Guid chatId);

    /// <summary>
    /// Получить сообщения комнаты.
    /// </summary>
    /// <param name="chatId">Идентификатор комнаты.</param>
    /// <returns>Список сообщений.</returns>
    Task<List<ChatMessageDto>> GetMessagesByRoomIdAsync(Guid chatId);

    /// <summary>
    /// Получить последние <paramref name="count"/> сообщений комнаты.
    /// </summary>
    /// <param name="roomId">Идентификатор комнаты.</param>
    /// <param name="count">Количество сообщений.</param>
    /// <returns>Список сообщений.</returns>
    Task<List<ChatMessageDto>> GetLastMessagesAsync(Guid roomId, int count);

    /// <summary>
    /// Получить сообщение по идентификатору.
    /// </summary>
    /// <param name="messageId">Идентификатор сообщения.</param>
    /// <returns>Сообщение.</returns>
    Task<ChatMessageDto?> GetByIdAsync(Guid messageId);

    /// <summary>
    /// Удалить сообщение.
    /// </summary>
    /// <param name="messageId">Идентификатор сообщения.</param>
    /// <returns>Идентификатор удаленного сообщения.</returns>
    Task<Guid> DeleteAsync(Guid messageId);
}