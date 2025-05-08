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
    /// <param name="message">Сообщение.</param>
    /// <param name="senderId">Идентификатор отправителя.</param>
    /// <param name="chatId">Идентификатор чата.</param>
    /// <returns>Идентификатор сообщения.</returns>
    public async Task<Guid> AddAsync(string message, Guid senderId, Guid chatId)
    {
        var messageDto = new ChatMessageDto()
        {
            Id = Guid.NewGuid(),
            SenderId = senderId,
            ChatRoomId = chatId,
            Message = message,
            SentAt = DateTime.UtcNow,
        };
        
        return await _chatMessageRepository.AddAsync(messageDto);
    }

    /// <summary>
    /// Получить сообщения комнаты.
    /// </summary>
    /// <param name="chatId">Идентификатор комнаты.</param>
    /// <returns>Список сообщений.</returns>
    public async Task<List<ChatMessageDto>> GetMessagesByRoomIdAsync(Guid chatId)
    {
        return await _chatMessageRepository.GetMessagesByRoomIdAsync(chatId);
    }

    /// <summary>
    /// Получить последние <paramref name="count"/> сообщений комнаты.
    /// </summary>
    /// <param name="roomId">Идентификатор комнаты.</param>
    /// <param name="count">Количество сообщений.</param>
    /// <returns>Список сообщений.</returns>
    public async Task<List<ChatMessageDto>> GetLastMessagesAsync(Guid roomId, int count)
    {
        return await _chatMessageRepository.GetLastMessagesAsync(roomId, count);
    }

    /// <summary>
    /// Получить сообщение по идентификатору.
    /// </summary>
    /// <param name="messageId">Идентификатор сообщения.</param>
    /// <returns>Сообщение.</returns>
    public async Task<ChatMessageDto?> GetByIdAsync(Guid messageId)
    {
        return await _chatMessageRepository.GetByIdAsync(messageId);
    }

    /// <summary>
    /// Удалить сообщение.
    /// </summary>
    /// <param name="messageId">Идентификатор сообщения.</param>
    /// <returns>Идентификатор удаленного сообщения.</returns>
    public async Task<Guid> DeleteAsync(Guid messageId)
    {
        return await _chatMessageRepository.DeleteAsync(messageId);
    }
}