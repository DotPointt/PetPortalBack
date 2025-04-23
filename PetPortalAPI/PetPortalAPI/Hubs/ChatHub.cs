using Microsoft.AspNetCore.SignalR;
using PetPortalCore.Abstractions;

namespace PetPortalAPI.Hubs;

/// <summary>
/// Хаб чатов. 
/// </summary>
public class ChatHub : Hub<IChatClient>
{
    /// <summary>
    /// Присоединение к чату.
    /// </summary>
    /// <param name="chatRoomId">Идентификатор чата.</param>
    public async Task JoinRoom(Guid chatRoomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomId.ToString());
    }

    /// <summary>
    /// Выход из чата.
    /// </summary>
    /// <param name="chatRoomId">Идентификатор чата.</param>
    public async Task LeaveRoom(Guid chatRoomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatRoomId.ToString());
    }
}
