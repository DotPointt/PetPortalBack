using PetPortalCore.DTOs;

namespace PetPortalCore.Abstractions.Repositories;

/// <summary>
/// Интерфейс репозитория для работы с сообщениями чатов.
/// </summary>
public interface IChatMessageRepository
{
    /// <summary>
    /// Записать сообщение в базу данных.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    /// <returns>Идентификатор сообщения.</returns>
    Task<Guid> AddAsync(ChatMessageDto message);

    /// <summary>
    /// Удалить сообщение из базы данных.
    /// </summary>
    /// <param name="id">Идентификатор сообщения.</param>
    /// <returns>Идентификатор удаленного сообщения.</returns>
    Task<Guid> DeleteAsync(Guid id);

    /// <summary>
    /// Получить сообщение по идентификатору. 
    /// </summary>
    /// <param name="id">Идентификатор сообщения.</param>
    /// <returns>Сообщение (или null пока).</returns>
    Task<ChatMessageDto> GetByIdAsync(Guid id);

    /// <summary>
    /// Получить сообщения по названию комнаты.
    /// </summary>
    /// <param name="chatRoom">Название чата.</param>
    /// <returns>Список сообщений чата.</returns>
    Task<List<ChatMessageDto>> GetMessagesByRoomAsync(string chatRoom);

    /// <summary>
    /// Получить <paramref name="count"/> сообщений чата <paramref name="chatRoom"/>.
    /// </summary>
    /// <param name="chatRoom">Название чата.</param>
    /// <param name="count">Количество сообщений.</param>
    /// <returns>Список сообщений по параметрам.</returns>
    Task<List<ChatMessageDto>> GetLastMessagesAsync(string chatRoom, int count);
}