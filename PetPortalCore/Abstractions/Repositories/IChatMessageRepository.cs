using PetPortalCore.DTOs;

namespace PetPortalCore.Abstractions.Repositories;

public interface IChatMessageRepository
{
    Task<Guid> AddAsync(ChatMessageDto message);
    Task<List<ChatMessageDto>> GetMessagesByRoomIdAsync(Guid roomId);
    Task<List<ChatMessageDto>> GetLastMessagesAsync(Guid roomId, int count);
    Task<ChatMessageDto?> GetByIdAsync(Guid messageId);
    Task<Guid> DeleteAsync(Guid messageId);
}