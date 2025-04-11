using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.DTOs;

namespace PetPortalApplication.Services;

/// <summary>
/// Сервис для работы с сообщениями в чатах.
/// </summary>
public class ChatMessageService : IChatMessageService
{
    /// <summary>
    /// Репозиторий для работы с сообщениями в чатах.
    /// </summary>
    private readonly IChatMessageRepository _chatMessageRepository;
    
    /// <summary>
    /// Конструктор сервиса обработки сообщений в чатах.
    /// </summary>
    /// <param name="chatMessageRepository">Репозиторий работы с сообщениями в чатах.</param>
    public ChatMessageService(IChatMessageRepository chatMessageRepository)
    {
       _chatMessageRepository = chatMessageRepository; 
    }

    /// <summary>
    /// Записать сообщение в базу данных.
    /// </summary>
    /// <param name="chatMessage">Сообщение.</param>
    /// <returns>Идентификатор сообщения.</returns>
    public async Task<Guid> AddAsync(ChatMessageDto chatMessage)
    {
        return await _chatMessageRepository.AddAsync(chatMessage);
    }

    /// <summary>
    /// Удалить сообщение из базы данных.
    /// </summary>
    /// <param name="chatMessageId">Идентификатор сообщения.</param>
    /// <returns>Идентификатор удаленного сообщения.</returns>
    public async Task<Guid> DeleteAsync(Guid chatMessageId)
    {
        return await _chatMessageRepository.DeleteAsync(chatMessageId);
    }

    /// <summary>
    /// Получить сообщения по названию комнаты.
    /// </summary>
    /// <param name="roomName">Название чата.</param>
    /// <returns>Список сообщений чата.</returns>
    public async Task<List<ChatMessageDto>> GetMessagesByRoomAsync(string roomName)
    {
        return await _chatMessageRepository.GetMessagesByRoomAsync(roomName);
    }

    /// <summary>
    /// Получить <paramref name="count"/> сообщений чата <paramref name="chatRoom"/>.
    /// </summary>
    /// <param name="chatRoom">Название чата.</param>
    /// <param name="count">Количество сообщений.</param>
    /// <returns>Список сообщений по параметрам.</returns>
    public async Task<List<ChatMessageDto>> GetLastMessagesAsync(string chatRoom, int count)
    {
        return await _chatMessageRepository.GetLastMessagesAsync(chatRoom, count);
    }
    
    /// <summary>
    /// Получить сообщение по идентификатору. 
    /// </summary>
    /// <param name="chatMessageId">Идентификатор сообщения.</param>
    /// <returns>Сообщение (или null пока).</returns>
    public async Task<ChatMessageDto> GetByIdAsync(Guid chatMessageId)
    {
        return await _chatMessageRepository.GetByIdAsync(chatMessageId);
    }
    
    
}