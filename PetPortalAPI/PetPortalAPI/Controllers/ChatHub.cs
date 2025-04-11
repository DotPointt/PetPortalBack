using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using PetPortalCore.Abstractions;
using PetPortalCore.Abstractions.Services;
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
    /// Сервис по обработке сообщений.
    /// </summary>
    private readonly IChatMessageService _chatMessageService;

    /// <summary>
    /// Количество сообщений для загрузки.
    /// </summary>
    private const int MessagesCountToLoad = 50;

    /// <summary>
    /// Конструктор контроллера чатов.
    /// </summary>
    /// <param name="cache">Кэш для чатов.</param>
    /// <param name="chatMessageService">Сервис по обработке сообщений.</param>
    public ChatHub(IDistributedCache cache, IChatMessageService chatMessageService)
    {
        _cache = cache;
        _chatMessageService = chatMessageService;
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
        
        var recentMessages = await _chatMessageService.GetMessagesByRoomAsync(connection.ChatRoom);
        if (recentMessages.Count != 0)
        {
            foreach (var msg in recentMessages.OrderBy(m => m.SentAt))
            {
                await Clients.Caller.ReceiveMessage(msg.Username, msg.Message);
            }   
        }
        
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

            var messageDto = new ChatMessageDto()
            {
                Id = Guid.NewGuid(),
                ChatRoom = connection.ChatRoom,
                Username = connection.UserName,
                Message = message,
                SentAt = DateTime.UtcNow
            };
            
            await _chatMessageService.AddAsync(messageDto);
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