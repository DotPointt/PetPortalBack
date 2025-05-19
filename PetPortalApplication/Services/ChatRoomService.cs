using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.DTOs;

namespace PetPortalApplication.Services;

/// <summary>
/// Сервис для работы с чатами.
/// </summary>
public class ChatRoomService : IChatRoomService
{
    /// <summary>
    /// Репозиторий для работы с чатами.
    /// </summary>
    private readonly IChatRoomRepository _chatRoomRepository;

    /// <summary>
    /// Конструктор сервиса обработки чатов.
    /// </summary>
    /// <param name="chatRoomRepository">Репозиторий для работы с чатами.</param>
    public ChatRoomService(IChatRoomRepository chatRoomRepository)
    {
        _chatRoomRepository = chatRoomRepository;
    }

    /// <summary>
    /// Получить чат по идентификатору.
    /// </summary>
    /// <param name="roomId">Идентификатор чата.</param>
    /// <returns>Чат.</returns>
    public async Task<ChatRoomDto?> GetByIdAsync(Guid roomId)
    {
        return await _chatRoomRepository.GetByIdAsync(roomId);
    }

    /// <summary>
    /// Получить чаты пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Список чатов.</returns>
    public async Task<List<ChatRoomDto>> GetUserChatRoomsAsync(Guid userId)
    {
        return await _chatRoomRepository.GetUserChatRoomsAsync(userId);
    }

    /// <summary>
    /// Получить идентификатор чата по имени.
    /// </summary>
    /// <param name="name">Имя чата.</param>
    /// <returns>Чат.</returns>
    public async Task<Guid?> GetChatRoomIdByNameAsync(string name)
    {
        return await _chatRoomRepository.GetChatRoomIdByNameAsync(name);
    }

    /// <summary>
    /// Создать чат.
    /// </summary>
    /// <param name="name">Имя чата.</param>
    /// <param name="userIds">Идентификаторы пользователей.</param>
    /// <returns>Созданный чат.</returns>
    public async Task<ChatRoomDto> CreateNamedChatAsync(string name, List<Guid> userIds)
    {
        return await _chatRoomRepository.CreateNamedChatAsync(name, userIds);
    }
}