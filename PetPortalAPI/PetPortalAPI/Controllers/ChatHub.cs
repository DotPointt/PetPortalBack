using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using PetPortalCore.Abstractions;
using PetPortalCore.DTOs;

namespace PetPortalAPI.Controllers;

/// <summary>
/// Контроллер чатов. 
/// </summary>
public class ChatHub : Hub<IChatClient>
{
    /// <summary>
    /// Кэш для чата.
    /// </summary>
    private readonly IDistributedCache _cache;

    /// <summary>
    /// Конструктор контроллера чатов.
    /// </summary>
    /// <param name="cache">Кэш для чатов.</param>
    public ChatHub(IDistributedCache cache)
    {
        _cache = cache;
    }
    
    /// <summary>
    /// Подключение к чату.
    /// </summary>
    /// <param name="connection">Данные подключающегося пользователя.</param>
    public async Task JoinChat(UserChatConnection connection)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatRoom);
        
        var stringConnection = JsonSerializer.Serialize(connection);
        
        await _cache.SetStringAsync(Context.ConnectionId, stringConnection);
        
        await Clients
            .Group(connection.ChatRoom)
            .ReceiveMessage("Admin", $"{connection.UserName} присоединился к чату");
    }       

    /// <summary>
    /// Отправка сообщения.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    public async Task SendMessage(string message)
    {
        var stringConnection = await _cache.GetAsync(Context.ConnectionId);

        var connection = JsonSerializer.Deserialize<UserChatConnection>(stringConnection);

        if (connection is not null)
        {
            await Clients
                .Group(connection.ChatRoom)
                .ReceiveMessage(connection.UserName, message);
        }
    }
    
    /// <summary>
    /// Отключиться от чата.
    /// </summary>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var stringConnection = await _cache.GetAsync(Context.ConnectionId);
        var connection = JsonSerializer.Deserialize<UserChatConnection>(stringConnection);

        if (connection is not null)
        {
            await _cache.RemoveAsync(Context.ConnectionId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, connection.ChatRoom);

            await Clients
                .Group(connection.ChatRoom)
                .ReceiveMessage("Admin", $"{connection.UserName} покинул чат");
        }

        await base.OnDisconnectedAsync(exception);
    }
}