using PetPortalCore.DTOs;

namespace PetPortalCore.Abstractions.Repositories;

public interface IChatRoomRepository
{
    Task<ChatRoomDto?> GetByIdAsync(Guid roomId);
    Task<List<ChatRoomDto>> GetUserChatRoomsAsync(Guid userId);
    Task<Guid?> GetChatRoomIdByNameAsync(string name);
    Task<ChatRoomDto> CreateNamedChatAsync(string name, List<Guid> userIds);
}
