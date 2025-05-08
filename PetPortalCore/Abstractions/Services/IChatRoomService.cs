using PetPortalCore.DTOs;

namespace PetPortalCore.Abstractions.Services;

/// <summary>
///  Интерфейс сервиса для работы с чатами.
/// </summary>
public interface IChatRoomService
{
    /// <summary>
    /// Получить чат по идентификатору.
    /// </summary>
    /// <param name="roomId">Идентификатор чата.</param>
    /// <returns>Чат.</returns>
    Task<ChatRoomDto?> GetByIdAsync(Guid roomId);

    /// <summary>
    /// Получить чаты пользователя.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Список чатов.</returns>
    Task<List<ChatRoomDto>> GetUserChatRoomsAsync(Guid userId);

    /// <summary>
    /// Получить идентификатор чата по имени.
    /// </summary>
    /// <param name="name">Имя чата.</param>
    /// <returns>Чат.</returns>
    Task<Guid?> GetChatRoomIdByNameAsync(string name);

    /// <summary>
    /// Создать чат.
    /// </summary>
    /// <param name="name">Имя чата.</param>
    /// <param name="userIds">Идентификаторы пользователей.</param>
    /// <returns>Созданный чат.</returns>
    Task<ChatRoomDto> CreateNamedChatAsync(string name, List<Guid> userIds);
}