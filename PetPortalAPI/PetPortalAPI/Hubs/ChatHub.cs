using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PetPortalCore.Abstractions;

namespace PetPortalAPI.Hubs;

/// <summary>
/// Хаб чатов.
/// </summary>
[Authorize]
public class ChatHub : Hub<IChatClient>
{
    public static string UserGroup(Guid userId) => $"user:{userId:D}";

    public override async Task OnConnectedAsync()
    {
        var claim = Context.User?.FindFirst("sub")
                    ?? Context.User?.FindFirst(ClaimTypes.NameIdentifier);
        if (claim != null && Guid.TryParse(claim.Value, out var userId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, UserGroup(userId));
        }

        await base.OnConnectedAsync();
    }

    /// <summary>
    /// Присоединение к комнате чата.
    /// </summary>
    public async Task JoinRoom(Guid chatRoomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomId.ToString());
    }

    /// <summary>
    /// Выход из комнаты чата.
    /// </summary>
    public async Task LeaveRoom(Guid chatRoomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatRoomId.ToString());
    }
}
