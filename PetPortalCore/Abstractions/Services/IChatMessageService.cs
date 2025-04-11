using PetPortalCore.DTOs;

namespace PetPortalCore.Abstractions.Services;

/// <summary>
/// Интерфейс сервиса для работы с сообщениями в чатах.
/// </summary>
public interface IChatMessageService
{
    /// <summary>
    /// Записать сообщение в базу данных.
    /// </summary>
    /// <param name="chatMessage">Сообщение.</param>
    /// <returns>Идентификатор сообщения.</returns>
    Task<Guid> AddAsync(ChatMessageDto chatMessage);

    /// <summary>
    /// Удалить сообщение из базы данных.
    /// </summary>
    /// <param name="chatMessageId">Идентификатор сообщения.</param>
    /// <returns>Идентификатор удаленного сообщения.</returns>
    Task<Guid> DeleteAsync(Guid chatMessageId);

    /// <summary>
    /// Получить сообщения по названию комнаты.
    /// </summary>
    /// <param name="roomName">Название чата.</param>
    /// <returns>Список сообщений чата.</returns>
    Task<List<ChatMessageDto>> GetMessagesByRoomAsync(string roomName);

    /// <summary>
    /// Получить <paramref name="count"/> сообщений чата <paramref name="chatRoom"/>.
    /// </summary>
    /// <param name="chatRoom">Название чата.</param>
    /// <param name="count">Количество сообщений.</param>
    /// <returns>Список сообщений по параметрам.</returns>
    Task<List<ChatMessageDto>> GetLastMessagesAsync(string chatRoom, int count);

    /// <summary>
    /// Получить сообщение по идентификатору. 
    /// </summary>
    /// <param name="chatMessageId">Идентификатор сообщения.</param>
    /// <returns>Сообщение (или null пока).</returns>
    Task<ChatMessageDto> GetByIdAsync(Guid chatMessageId);
}