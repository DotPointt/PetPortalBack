using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.DTOs;

namespace PetPortalApplication.Services;

/// <summary>
/// Сервис для работы с сообщениями в чатах.
/// </summary>
public class ChatMessageService : IChatMessageService
{
    private readonly IChatMessageRepository _chatMessageRepository;
    
    public ChatMessageService(IChatMessageRepository chatMessageRepository)
    {
       _chatMessageRepository = chatMessageRepository; 
    }

    public async Task<ChatMessageDto> AddAsync(string message, Guid senderId, Guid chatId)
    {
        var messageDto = new ChatMessageDto()
        {
            Id = Guid.NewGuid(),
            SenderId = senderId,
            ChatRoomId = chatId,
            Message = message,
            SentAt = DateTime.UtcNow,
        };
        
        await _chatMessageRepository.AddAsync(messageDto);
        var saved = await _chatMessageRepository.GetByIdAsync(messageDto.Id);
        return saved ?? messageDto;
    }

    public async Task<List<ChatMessageDto>> GetMessagesByRoomIdAsync(Guid chatId)
    {
        return await _chatMessageRepository.GetMessagesByRoomIdAsync(chatId);
    }

    public async Task<List<ChatMessageDto>> GetLastMessagesAsync(Guid roomId, int count)
    {
        return await _chatMessageRepository.GetLastMessagesAsync(roomId, count);
    }

    public async Task<ChatMessageDto?> GetByIdAsync(Guid messageId)
    {
        return await _chatMessageRepository.GetByIdAsync(messageId);
    }

    public async Task<Guid> DeleteAsync(Guid messageId)
    {
        return await _chatMessageRepository.DeleteAsync(messageId);
    }
}
